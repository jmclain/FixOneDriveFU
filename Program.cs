using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace FixOneDriveFU {

	/////////////////////////////////////////////////////////////////////////////

	class Program {

		const string TARGET = "-JR64";
		const string TARGET_DOT = "-JR64.";
		
		const string MINUS_ONE = "-1";
		const string MINUS_ONE_DOT = "-1.";
		
		const char DOT_CHAR = '.';
		const string DOT_STRING = ".";


		/////////////////////////////////////////////////////////////////////////////

		static void Exit( int code = 0 )
		{
			Environment.Exit( code );
		}


		/////////////////////////////////////////////////////////////////////////////

		static void Write( string fmt, params object [] args )
		{
			Console.WriteLine( fmt, args );
		}


		/////////////////////////////////////////////////////////////////////////////

		static string GetBaseName( string fileName, string target)
		{
			// ******
			if( target.EndsWith( DOT_STRING ) ) {
				//
				// somewhere in string, need to return with dot:
				//
				//   somefile-1.txt ---- somefile.txt
				//
				return fileName.Replace( target, DOT_STRING );
			}

			//
			// otherwise ends with 'target'
			//
			//    somefile-1 ---- somefile
			//
			return fileName.Substring( 0, fileName.Length - target.Length );
		}

	
		/////////////////////////////////////////////////////////////////////////////

		static bool FixFile( string fileName, string target)
		{
			if( !File.Exists( fileName ) ) {
				//
				// been deleted ?
				return false;
			}

			if( !fileName.Contains( target ) ) {
				return false;
			}

			var baseFile = GetBaseName( fileName, target );

			if( File.Exists( baseFile ) ) {
				Write( "replacing {0} with {1}", Path.GetFileName(baseFile), Path.GetFileName(fileName) );
				File.Delete( baseFile );
				File.Move( fileName, baseFile );
				return true;
			}

			// ******
			Write( "{0} does not exist", baseFile );
			return false;
		}


		/////////////////////////////////////////////////////////////////////////////

		static string CheckFile( string name )
		{
			//
			// "-JR64.
			//
			if( name.Contains( TARGET_DOT ) ) {
				return TARGET_DOT;
			}

			if( name.EndsWith( TARGET ) ) {
				return TARGET;
			}

			//
			// "-1."
			//
			if( name.Contains( MINUS_ONE_DOT ) ) {
				return MINUS_ONE_DOT;
			}

			if( name.EndsWith( MINUS_ONE ) ) {
				return MINUS_ONE;
			}

			return null;
		}


		/////////////////////////////////////////////////////////////////////////////

		static void FixFiles( string dirPath )
		{
			var dirs = Directory.GetDirectories( dirPath );
			foreach( var dir in dirs ) {
				FixFiles( dir );
			}

			var files = Directory.GetFiles( dirPath );
			foreach( var file in files ) {
				var innerStrTarget = CheckFile( file );
				if( !string.IsNullOrEmpty( innerStrTarget ) ) {
					FixFile( file, innerStrTarget );
				}
			}

		}


		/////////////////////////////////////////////////////////////////////////////

		//static void FixFiles( string dirPath )
		//{
		//	var dirs = Directory.GetDirectories( dirPath );
		//	foreach( var dir in dirs ) {
		//		FixFiles( dir );
		//	}

		//	var files = Directory.GetFiles( dirPath, "*" + TARGET + ".*" );
		//	foreach( var file in files ) {
		//		var baseFile = file.Replace( TARGET, "" );
		//		if( File.Exists( baseFile ) ) {
		//			File.Delete( baseFile );
		//			File.Move( file, baseFile );
		//		}
		//	}

		//}


		/////////////////////////////////////////////////////////////////////////////

		//static int totalDirectories = 0;
		//static int totalFiles = 0;
		//static int noBaseFiles = 0;

		//static void RecurseAndDisplay( string dirPath )
		//{
		//	++totalDirectories;

		//	//Write( "directory: {0}", dirPath );

		//	var dirs = Directory.GetDirectories( dirPath );
		//	foreach( var dir in dirs ) {
		//		RecurseAndDisplay( dir );
		//	}

		//	//var files = Directory.GetFiles( dirPath, "*" + TARGET + ".*" );
		//	var files = Directory.GetFiles( dirPath );

		//	foreach( var file in files ) {
		//		++totalFiles;
		//		//Write( "  {0}", file );
		//		var baseFile = file.Replace( TARGET, "" );
		//		if( !File.Exists( baseFile ) ) {
		//			++noBaseFiles;
		//			Write( "  {0} does not exist", baseFile );
		//		}
		//	}

		//}


		/////////////////////////////////////////////////////////////////////////////

		static void Run()
		{
			// ******
			Write( "current directory: {0}", Environment.CurrentDirectory );
			//Exit();

			//var topLevelFiles = Directory.GetDirectories( Environment.CurrentDirectory );	//, "*{target}.*", SearchOption.AllDirectories );

			//RecurseAndDisplay( Environment.CurrentDirectory );

			//Write( "total directories {0}", totalDirectories );
			//Write( "total files       {0}", totalFiles );
			//Write( "have no base file {0}", noBaseFiles );

			//var key = Console.ReadKey();
			//if( ConsoleKey.Y == key.Key ) {
			//	FixFiles( Environment.CurrentDirectory );
			//}

			FixFiles( Environment.CurrentDirectory );

			Exit();

		}


		/////////////////////////////////////////////////////////////////////////////

		static void Main( string [] args )
		{
			Run();
		}

	}
}
