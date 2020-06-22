using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
//using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Runtime.InteropServices;



namespace set_time{
  class Program{

    public static void logme(bool is_do_something, string str){
      if(is_do_something == false) return;
      Console.Error.WriteLine(str);
    }
    
    public static void Main(string[] args){
      string help = "set_time.exe v" + Process.GetCurrentProcess().MainModule.FileVersionInfo.FileVersion               + "\r\n"
                  + "Runs with .Net v" + Environment.Version                                                              + "\r\n"
                  + "\r\n"
                  + "a command-line program to set file or folder data/time meta-data."                                 + "\r\n"
                  + "\r\n"
                  + "--target \"STRING\""                   + "\t\t\t" + "specify a path to a file or folder."                     + "\r\n"
                  + "--set-time-creation NUMBER"        + "\t\t" + "set the target creation-time."                             + "\r\n"
                  + "--set-time-last-access NUMBER"     + "\t\t" + "set the target last-access time."                          + "\r\n"
                  + "--set-time-last-modified NUMBER"   + "\t" + "set the target last-modified time."                          + "\r\n"
                  + "--verbose"                  + "\t\t\t" + "additional information (in stderr)."                     + "\r\n"
                  + "--help"                     + "\t\t\t\t" + "show this help screen."                                + "\r\n"
                  + "\r\n"
                  + "1. You must specify target and at least one (can be more) of the --set-time-* arguments."          + "\r\n"
                  + "2. Target can be a single file or a single folder, it is best to wrap the arguments with \"\" ."   + "\r\n"
                  + "3. Specify time as a long (int64) number (a.k.a Unix-Time) - for example: 1592859319994."          + "\r\n"
                  + "4. All times are UTC! (not \"your machine-time\")."                                                + "\r\n"
                  + "\r\n"
                  ;
      string[] args_lowcase = (string[])Array.ConvertAll(args, d => d.ToLower());
      int index_arg_verbose                = Array.IndexOf(args_lowcase, "--verbose"                 );
      bool is_verbose = (index_arg_verbose != -1);
      
      int index_arg_help                   = Array.IndexOf(args_lowcase, "--help"                    );
      int index_arg_target                 = Array.IndexOf(args_lowcase, "--target"                  );
      int index_arg_set_time_creation      = Array.IndexOf(args_lowcase, "--set-time-creation"       );
      int index_arg_set_time_last_access   = Array.IndexOf(args_lowcase, "--set-time-last-access"    );
      int index_arg_set_time_last_modified = Array.IndexOf(args_lowcase, "--set-time-last-modified"  );      
      string value_target;
      FileAttributes attributes;
      bool is_exist;
      bool is_directory;
      DateTime epoch = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
      DateTime value_time_creation      = epoch; //asignning dummy value.
      DateTime value_time_last_access   = epoch; //asignning dummy value.
      DateTime value_time_last_modified = epoch; //asignning dummy value.
      
      if(index_arg_verbose != -1){ logme(true, "[INFO] got --verbose - additional information will be written (to stderr)."); }
      
      //case #0 - user specify --help (show text help in stdout and quit with exit-code 0).
      if(index_arg_help != -1){ Console.Out.WriteLine(help); Environment.Exit(0); }
      
      //error case #1 - no "--target"
      if(index_arg_target == -1){ logme(is_verbose, "[ERROR] missing --target argument."); Environment.Exit(111); }
      
      //error case #2 - missing time-setting.
      if(index_arg_set_time_creation == -1 && index_arg_set_time_last_access == -1 && index_arg_set_time_last_modified == -1){ logme(is_verbose, "[ERROR] missing at least one (can use more) time-setting argument: --set-time-creation, --set-time-last-access, --set-time-last-modified."); Environment.Exit(222); }

      logme(is_verbose, "[INFO] got --target arg.");
      value_target = args[index_arg_target + 1].Trim('\"');
      logme(is_verbose, "[INFO] target value raw: [" + value_target + "].");
      value_target = Path.GetFullPath(value_target);
      logme(is_verbose, "[INFO] target fixed to absolute-path: [" + value_target + "].");
      
      is_exist = File.Exists(value_target) || Directory.Exists(value_target);
      if(is_exist == false){ logme(is_verbose, "[ERROR] target [" + value_target + "] does not exist."); Environment.Exit(333); }
      logme(is_verbose, "[INFO] target exist.");
      
      attributes = File.GetAttributes(value_target);      
      is_directory = attributes.HasFlag(FileAttributes.Directory);
      logme(is_verbose, "[INFO] is a directory? " + is_directory);
    
      //pre-process string into long to DateTime
      if(index_arg_set_time_creation != -1){
        logme(is_verbose, "[INFO] got --set-time-creation arg.");
        value_time_creation = epoch.AddSeconds(  Convert.ToInt64( args[index_arg_set_time_creation + 1].Trim('\"') )  );
        logme(is_verbose, "[INFO] time-creation value DateTime (UTC) [" + value_time_creation + "].");
      }
      
      if(index_arg_set_time_last_access != -1){
        logme(is_verbose, "[INFO] got --set-time-last-access arg.");
        value_time_last_access = epoch.AddSeconds(  Convert.ToInt64( args[index_arg_set_time_last_access + 1].Trim('\"') )  );
        logme(is_verbose, "[INFO] time-last-access value DateTime (UTC) [" + value_time_last_access + "].");
      }

      if(index_arg_set_time_last_modified != -1){
        logme(is_verbose, "[INFO] got --set-time-last-modified arg.");
        value_time_last_modified = epoch.AddSeconds(  Convert.ToInt64( args[index_arg_set_time_last_modified + 1].Trim('\"') )  );
        logme(is_verbose, "[INFO] time-last-modified value DateTime (UTC) [" + value_time_last_modified + "].");
      }
     
      
      if(is_directory == true){
        logme(is_verbose, "[INFO] working on a directory.");
        if(index_arg_set_time_creation       != -1){ Directory.SetCreationTimeUtc(value_target,   value_time_creation);       logme(is_verbose, "[INFO] applied new creation-time successfully.");      }
        if(index_arg_set_time_last_access    != -1){ Directory.SetLastAccessTimeUtc(value_target, value_time_last_access);    logme(is_verbose, "[INFO] applied new last-access-time successfully.");   }
        if(index_arg_set_time_last_modified  != -1){ Directory.SetLastWriteTimeUtc(value_target,  value_time_last_modified);  logme(is_verbose, "[INFO] applied new last-modified-time successfully."); }
      }else{
        logme(is_verbose, "[INFO] working on a file.");
        if(index_arg_set_time_creation       != -1){      File.SetCreationTimeUtc(value_target,   value_time_creation);       logme(is_verbose, "[INFO] applied new creation-time successfully.");      }
        if(index_arg_set_time_last_access    != -1){      File.SetLastAccessTimeUtc(value_target, value_time_last_access);    logme(is_verbose, "[INFO] applied new last-access-time successfully.");   }
        if(index_arg_set_time_last_modified  != -1){      File.SetLastWriteTimeUtc(value_target,  value_time_last_modified);  logme(is_verbose, "[INFO] applied new last-modified-time successfully."); }
      }
      
      logme(is_verbose, "[INFO] Done.");
      Environment.Exit(0);
    }
  }
}
