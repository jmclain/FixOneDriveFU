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

		static void RecurseAndDisplay( string dirPath )
		{
			Write( "directory: {0}", dirPath );

			var dirs = Directory.GetDirectories( dirPath );
			foreach( var dir in dirs ) {
				RecurseAndDisplay( dir );
			}

			Write( "files for {0}", dirPath );

			var files = Directory.GetFiles( dirPath, "*" + TARGET + ".*" );
			foreach( var file in files ) {
				Write( "  {0}", file );
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

			Exit();

		}


		/////////////////////////////////////////////////////////////////////////////

		static void Main( string [] args )
		{
			Run();
		}

	}
}
