using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class GameScanner
{
    public static List<string> Scan(string root, bool recursive, string self, CancellationToken token, List<string> warnings)
    {
        var result = new List<string>();
        var pending = new Stack<string>();
        pending.Push(Path.GetFullPath(root));
        while (pending.Count > 0)
        {
            token.ThrowIfCancellationRequested();
            string folder = pending.Pop();
            try
            {
                foreach (string file in Directory.GetFiles(folder))
                {
                    token.ThrowIfCancellationRequested();
                    if (string.Equals(Path.GetExtension(file), ".exe", StringComparison.OrdinalIgnoreCase)
                        && !string.Equals(Path.GetFileName(file), "nobk.exe", StringComparison.OrdinalIgnoreCase)
                        && !string.Equals(Path.GetFileName(file), "gameselector.exe", StringComparison.OrdinalIgnoreCase)
                        && !string.Equals(Path.GetFullPath(file), self, StringComparison.OrdinalIgnoreCase)) result.Add(file);
                }
                if (recursive)
                    foreach (string child in Directory.GetDirectories(folder))
                    {
                        if ((File.GetAttributes(child) & FileAttributes.ReparsePoint) == 0) pending.Push(child);
                    }
            }
            catch (UnauthorizedAccessException) { warnings.Add(folder); }
            catch (IOException) { warnings.Add(folder); }
        }
        result.Sort(StringComparer.OrdinalIgnoreCase);
        return result;
    }
    public static ProcessStartInfo LaunchInfo(string file, string arguments)
    {
        return new ProcessStartInfo { FileName = Path.GetFullPath(file), WorkingDirectory = Path.GetDirectoryName(Path.GetFullPath(file)), Arguments = arguments, UseShellExecute = false };
    }
}

public sealed class FolderMemory
{
    readonly string file;
    public FolderMemory(string settingsRoot, string executablePath)
    {
        // Windows filenames are case-insensitive; location deliberately does not affect identity.
        string name = Path.GetFileName(executablePath).ToUpperInvariant();
        using (var hash = SHA256.Create())
            file = Path.Combine(settingsRoot, BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(name))).Replace("-", "") + ".folder");
    }
    public string Load()
    {
        try
        {
            if (!File.Exists(file)) return null;
            string folder = File.ReadAllText(file, Encoding.UTF8);
            return Directory.Exists(folder) ? folder : null;
        }
        catch (IOException) { return null; }
        catch (UnauthorizedAccessException) { return null; }
        catch (ArgumentException) { return null; }
    }
    public void Save(string folder)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(file));
        string temp = file + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            File.WriteAllText(temp, Path.GetFullPath(folder), Encoding.UTF8);
            if (File.Exists(file)) File.Replace(temp, file, null);
            else File.Move(temp, file);
        }
        finally { if (File.Exists(temp)) File.Delete(temp); }
    }
}

public sealed class Launcher : Form
{
    readonly Label folderLabel = new Label { AutoSize = true, Text = "Bir oyun klasörü seç." };
    readonly Label status = new Label { AutoSize = true, Text = "" };
    readonly FolderMemory memory = new FolderMemory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GameSelector", "Folders"), Application.ExecutablePath);
    readonly ListBox games = new ListBox { Dock = DockStyle.Fill, HorizontalScrollbar = true };
    readonly Button pick = new Button { Text = "Klasör seç", AutoSize = true };
    readonly Button refresh = new Button { Text = "Yenile", AutoSize = true };
    readonly Button launch = new Button { Text = "Seçili EXE'yi çalıştır", AutoSize = true, Enabled = false };
    readonly TextBox arguments = new TextBox { Width = 300 };
    string folder;
    CancellationTokenSource scan;
    bool running;

    public Launcher()
    {
        Text = Path.GetFileNameWithoutExtension(Application.ExecutablePath) + " • Oyun klasörü seçici";
        Size = new Size(800, 520); MinimumSize = new Size(600, 380);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 10);
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(14), ColumnCount = 1, RowCount = 5 };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var actions = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill };
        actions.Controls.AddRange(new Control[] { pick, refresh });
        var bottom = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill };
        bottom.Controls.AddRange(new Control[] { new Label { Text = "Başlatma parametreleri (isteğe bağlı):", AutoSize = true }, arguments, launch });
        layout.Controls.Add(actions); layout.Controls.Add(folderLabel); layout.Controls.Add(games); layout.Controls.Add(bottom); layout.Controls.Add(status);
        Controls.Add(layout);
        pick.Click += delegate { PickFolder(); };
        refresh.Click += delegate { ScanFolder(); };
        games.SelectedIndexChanged += delegate { launch.Enabled = games.SelectedItem != null && !running; };
        games.DoubleClick += delegate { LaunchGame(); };
        launch.Click += delegate { LaunchGame(); };
        Shown += delegate
        {
            folder = memory.Load();
            if (folder == null) PickFolder();
            else { folderLabel.Text = folder; ScanFolder(); }
        };
        FormClosing += delegate(object sender, FormClosingEventArgs e)
        {
            if (running && MessageBox.Show("Oyun hâlâ çalışıyor. Başlatıcıyı kapatmak Steam'in oturum takibini etkileyebilir. Kapatılsın mı?", Text, MessageBoxButtons.YesNo) != DialogResult.Yes) e.Cancel = true;
            if (!e.Cancel && scan != null) scan.Cancel();
        };
    }
    void PickFolder()
    {
        using (var dialog = new FolderBrowserDialog { Description = "EXE dosyalarını görmek istediğin oyun klasörünü seç", ShowNewFolderButton = false, SelectedPath = folder ?? "" })
        {
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            folder = dialog.SelectedPath; folderLabel.Text = folder;
            try { memory.Save(folder); }
            catch (Exception ex) { MessageBox.Show(this, "Klasör seçildi ancak hatırlama kaydı yazılamadı: " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            ScanFolder();
        }
    }
    async void ScanFolder()
    {
        if (folder == null || running) return;
        if (scan != null) scan.Cancel();
        var current = scan = new CancellationTokenSource();
        string root = folder;
        games.Items.Clear(); launch.Enabled = false; status.Text = "EXE dosyaları aranıyor…";
        var warnings = new List<string>();
        try
        {
            var files = await Task.Run(() => GameScanner.Scan(root, true, Application.ExecutablePath, current.Token, warnings));
            if (current.IsCancellationRequested || IsDisposed) return;
            foreach (string file in files) games.Items.Add(new GameItem(file, file.Substring(root.TrimEnd(Path.DirectorySeparatorChar).Length + 1)));
            status.Text = files.Count == 0 ? "Bu klasörde ve alt klasörlerinde EXE bulunamadı." : files.Count + " EXE bulundu (alt klasörler dahil). Çalıştırmak istediğini seç.";
            if (warnings.Count > 0) status.Text += "  Erişilemeyen klasör: " + warnings.Count;
            if (files.Count > 0) games.SelectedIndex = 0;
        }
        catch (OperationCanceledException) { }
        catch (Exception ex) { if (!IsDisposed && !current.IsCancellationRequested) status.Text = "Tarama hatası: " + ex.Message; }
    }
    async void LaunchGame()
    {
        var item = games.SelectedItem as GameItem;
        if (item == null || running) return;
        try
        {
            using (var process = Process.Start(GameScanner.LaunchInfo(item.Path, arguments.Text)))
            {
                if (process == null) throw new InvalidOperationException("İşlem başlatılamadı.");
                running = true; launch.Enabled = pick.Enabled = refresh.Enabled = arguments.Enabled = false;
                status.Text = "Oyun çalışıyor. Steam oturumu için başlatıcı açık kalıyor.";
                WindowState = FormWindowState.Minimized;
                await Task.Run(() => process.WaitForExit());
            }
            if (IsDisposed) return;
            WindowState = FormWindowState.Normal;
            status.Text = "Başlatılan EXE kapandı. Oyun ayrı bir işlem açtıysa başlatıcıyı açık bırak.";
        }
        catch (Exception ex) { if (!IsDisposed) MessageBox.Show(this, ex.Message, "Oyun başlatılamadı", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        finally
        {
            running = false;
            if (!IsDisposed) { pick.Enabled = refresh.Enabled = arguments.Enabled = true; launch.Enabled = games.SelectedItem != null; }
        }
    }
    sealed class GameItem
    {
        public readonly string Path; readonly string name;
        public GameItem(string path, string display) { Path = path; name = display; }
        public override string ToString() { return name; }
    }
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles(); Application.SetCompatibleTextRenderingDefault(false); Application.Run(new Launcher());
    }
}
