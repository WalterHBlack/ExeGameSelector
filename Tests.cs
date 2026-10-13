using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
class Tests {
 static void Check(bool ok, string name) { if (!ok) throw new Exception(name); Console.WriteLine("PASS: " + name); }
 static int Main(string[] args) {
  if(args.Length == 2 && args[0] == "--probe") { File.WriteAllText(args[1], Environment.CurrentDirectory + "\n" + Environment.GetEnvironmentVariable("NOBK_TEST_MARKER")); return 0; }
  string root = Path.Combine(Path.GetTempPath(), "NOBK test Türkçe " + Guid.NewGuid()); Directory.CreateDirectory(root);
  try {
   string sub = Path.Combine(root,"alt"); Directory.CreateDirectory(sub);
   string game = Path.Combine(root,"Sonic 4.EXE"); File.Copy(Assembly.GetExecutingAssembly().Location, game);
   File.Copy(typeof(GameScanner).Assembly.Location,Path.Combine(root,"gameselector.exe")); File.WriteAllText(Path.Combine(root,"NoBk.ExE"),""); File.WriteAllText(Path.Combine(root,"self.exe"),""); File.WriteAllText(Path.Combine(root,"fake.exe.txt"),"");
   File.WriteAllText(Path.Combine(sub,"second.exe"),""); File.WriteAllText(Path.Combine(sub,"nobk.exe"),"");
   var warnings = new List<string>();
   var direct = GameScanner.Scan(root,false,Path.Combine(root,"self.exe"),CancellationToken.None,warnings);
   Check(direct.Count == 1 && direct[0] == game,"Top-folder scope, EXE case, NOBK and self exclusion");
   var all = GameScanner.Scan(root,true,Path.Combine(root,"self.exe"),CancellationToken.None,warnings);
   Check(all.Count == 2 && all.Contains(Path.Combine(sub,"second.exe")),"Recursive opt-in and nested exclusions");
   var cts = new CancellationTokenSource(); cts.Cancel(); bool cancelled = false;
   try { GameScanner.Scan(root,true,"",cts.Token,warnings); } catch(OperationCanceledException) {cancelled=true;}
   Check(cancelled,"Cancellation");
   string settings = Path.Combine(root,"settings");
   var mania = new FolderMemory(settings,Path.Combine(root,"SonicMania.exe"));
   Check(mania.Load() == null,"New EXE name has no remembered folder");
   mania.Save(sub);
   Check(new FolderMemory(settings,Path.Combine(root,"SonicMania.exe")).Load() == sub,"Folder survives new instance");
   var nobk = new FolderMemory(settings,Path.Combine(root,"nobk.exe"));
   Check(nobk.Load() == null,"Rename isolates remembered folder");
   nobk.Save(root);
   Check(mania.Load() == sub && nobk.Load() == root,"EXE names retain independent selections");
   Check(new FolderMemory(settings,Path.Combine(sub,"SONICMANIA.EXE")).Load() == sub,"Same name, new location and letter case reuse selection");
   mania.Save(root);
   Check(mania.Load() == root && nobk.Load() == root,"Changing folder replaces saved selection");
   string removed = Path.Combine(root,"removed"); Directory.CreateDirectory(removed); mania.Save(removed); Directory.Delete(removed);
   Check(mania.Load() == null,"Missing saved folder falls back to picker");
   string output = Path.Combine(root,"probe output.txt"); Environment.SetEnvironmentVariable("NOBK_TEST_MARKER","inherited");
   using(var process = Process.Start(GameScanner.LaunchInfo(game,"--probe \"" + output + "\""))) { Check(process.WaitForExit(10000) && process.ExitCode == 0,"Real launch with Unicode/spaced paths and quoted arguments"); }
   Check(File.ReadAllText(output) == root + "\ninherited","Working directory and inherited environment"); return 0;
  } finally { Directory.Delete(root,true); }
 }
}

