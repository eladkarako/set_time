<h1>set_time</h1>

a C# command-line / console application to set the creation time, last-access time and modification time of either a file or a folder (directory), accepts a Unix-time ("long"/Int64 - seconds since epoch).

Set to compile with C# 5.0, and use .Net Framework runtime 4. The exe can run on either x86 or x64.

<hr/>

Here is a quick overview of the functionality. You can actually run the <code>example.cmd</code> to get it working (no need to fix paths, just run it).


<h3>will show help entries, output to stdout, then exit, exit-code 0.</h3>
<code>set_time.exe --help</code><br/>
<pre>
set_time.exe v1.0.7479.1677
Runs with .Net v4.0.30319.42000

a command-line program to set file or folder data/time meta-data.

--target "STRING"                       specify a path to a file or folder.
--set-time-creation NUMBER              set the target creation-time.
--set-time-last-access NUMBER           set the target last-access time.
--set-time-last-modified NUMBER set the target last-modified time.
--verbose                       additional information (in stderr).
--help                          show this help screen.

1. You must specify target and at least one (can be more) of the --set-time-* arguments.
2. Target can be a single file or a single folder, it is best to wrap the arguments with "" .
3. Specify time as a long (int64) number (a.k.a Unix-Time) - for example: 1592859319994.
4. All times are UTC! (not "your machine-time").
</pre>
<code>Exit-code: 0.</code></br/>

<hr/>

<h3>error, missing --target, verbose mode, exit-code 111.</h3>
<code>set_time.exe --verbose</code><br/>
<pre>
[INFO] got --verbose - additional information will be written (to stderr).
[ERROR] missing --target argument.
</pre>
<code>Exit-code: 111.</code><br/>

<hr/>

<h3>error, missing at least one --set-time-*, verbose mode, exit-code 222.</h3>
<code>set_time.exe --verbose --target "C:\Users\Elad\Documents\SharpDevelop Projects\set_time\dummy folder"</code><br/>
<pre>
[INFO] got --verbose - additional information will be written (to stderr).
[ERROR] missing at least one (can use more) time-setting argument: --set-time-creation, --set-time-last-access, --set-time-last-modified.
</pre>
<code>Exit-code: 222.</code><br/>

<hr/>

<h3>will set all timedates of folder, verbose mode, exit-code 0.</h3>
<code>set_time.exe --target "C:\Users\Elad\Documents\SharpDevelop Projects\set_time\dummy folder" --set-time-creation 123456789 --set-time-last-access 123456789 --set-time-last-modified 123456789 --verbose</code><br/>
<pre>
[INFO] got --verbose - additional information will be written (to stderr).
[INFO] got --target arg.
[INFO] target value raw: [C:\Users\Elad\Documents\SharpDevelop Projects\set_time\dummy folder].
[INFO] target fixed to absolute-path: [C:\Users\Elad\Documents\SharpDevelop Projects\set_time\dummy folder].
[INFO] target exist.
[INFO] is a directory? True
[INFO] got --set-time-creation arg.
[INFO] time-creation value DateTime (UTC) [29/11/1973 21:33:09].
[INFO] got --set-time-last-access arg.
[INFO] time-last-access value DateTime (UTC) [29/11/1973 21:33:09].
[INFO] got --set-time-last-modified arg.
[INFO] time-last-modified value DateTime (UTC) [29/11/1973 21:33:09].
[INFO] working on a directory.
[INFO] applied new creation-time successfully.
[INFO] applied new last-access-time successfully.
[INFO] applied new last-modified-time successfully.
[INFO] Done.
</pre>
<code>Exit-code: 0.</code><br/>

<hr/>

<h3>will set all timedates of file, verbose mode, exit code 0.</h3>
<code>set_time.exe --target "C:\Users\Elad\Documents\SharpDevelop Projects\set_time\dummy file.txt" --set-time-creation 123456789 --set-time-last-access 123456789 --set-time-last-modified 123456789 --verbose</code><br/>
<pre>
[INFO] got --verbose - additional information will be written (to stderr).
[INFO] got --target arg.
[INFO] target value raw: [C:\Users\Elad\Documents\SharpDevelop Projects\set_time\dummy file.txt].
[INFO] target fixed to absolute-path: [C:\Users\Elad\Documents\SharpDevelop Projects\set_time\dummy file.txt].
[INFO] target exist.
[INFO] is a directory? False
[INFO] got --set-time-creation arg.
[INFO] time-creation value DateTime (UTC) [29/11/1973 21:33:09].
[INFO] got --set-time-last-access arg.
[INFO] time-last-access value DateTime (UTC) [29/11/1973 21:33:09].
[INFO] got --set-time-last-modified arg.
[INFO] time-last-modified value DateTime (UTC) [29/11/1973 21:33:09].
[INFO] working on a file.
[INFO] applied new creation-time successfully.
[INFO] applied new last-access-time successfully.
[INFO] applied new last-modified-time successfully.
[INFO] Done.
</pre>
<code>Exit-code: 0.</code><br/>

<hr/>

<h2>Some notes</h2>

Exit codes are: 0 for success, 111 for missing --target, 222 for missing any -set-time-* args, 333 if the target does not exist on the file-system (or it is a network-root that needs an authentication..), and any other exit-code is an internal C# that was caused by an error (usually 1). This program does not handle too many error-cases and any exception would be thrown outside to the CMD. to speed up things, don't use the --verbose command-line switch if you don't really need it.