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

		static int totalDirectories = 0;
		static int totalFiles = 0;
		static int noBaseFiles = 0;

		static void RecurseAndDisplay( string dirPath )
		{
			++totalDirectories;

			//Write( "directory: {0}", dirPath );

			var dirs = Directory.GetDirectories( dirPath );
			foreach( var dir in dirs ) {
				RecurseAndDisplay( dir );
			}

			//Write( "files for {0}", dirPath );

			var files = Directory.GetFiles( dirPath, "*" + TARGET + ".*" );
			foreach( var file in files ) {
				++totalFiles;
				//Write( "  {0}", file );
				var baseFile = file.Replace( TARGET, "" );
				if( !File.Exists( baseFile ) ) {
					++noBaseFiles;
					Write( "  {0} does not exist", baseFile );
				}
			}

		}


		/////////////////////////////////////////////////////////////////////////////

		static void FixFiles( string dirPath )
		{
			var dirs = Directory.GetDirectories( dirPath );
			foreach( var dir in dirs ) {
				FixFiles( dir );
			}

			var files = Directory.GetFiles( dirPath, "*" + TARGET + ".*" );
			foreach( var file in files ) {
				var baseFile = file.Replace( TARGET, "" );
				if( File.Exists( baseFile ) ) {
					File.Delete( baseFile );
					File.Move( file, baseFile );
				}
			}

		}


		/////////////////////////////////////////////////////////////////////////////

		static void Run()
		{
			// ******
			Write( "current directory: {0}", Environment.CurrentDirectory );
			//Exit();

			//var topLevelFiles = Directory.GetDirectories( Environment.CurrentDirectory );	//, "*{target}.*", SearchOption.AllDirectories );

			RecurseAndDisplay( Environment.CurrentDirectory );

			Write( "total directories {0}", totalDirectories );
			Write( "total files       {0}", totalFiles );
			Write( "have no base file {0}", noBaseFiles );

			var key = Console.ReadKey();
			if( ConsoleKey.Y == key.Key ) {
				FixFiles( Environment.CurrentDirectory );
			}

			Exit();

		}


		/////////////////////////////////////////////////////////////////////////////

		static void Main( string [] args )
		{
			Run();
		}

	}
}
