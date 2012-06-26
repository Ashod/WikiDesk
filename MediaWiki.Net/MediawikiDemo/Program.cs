namespace MediawikiDemo
{
	using PHP.Core;
	using System;

	class Program
	{
		private static void Main(string[] args)
		{
			Test();
		}

		private static void Test()
		{
			//ScriptContext.CurrentContext
			ScriptContext context = ScriptContext.InitApplication(ApplicationContext.Default, null, null, null);

			// redirect PHP output to the console:
			context.Output = Console.Out; // Unicode text output
			context.OutputStream = Console.OpenStandardOutput(); // byte stream output

			context.Include("includes/WikiDeskStart.php", true);
			/*
						context.Include("includes/Defines.php", true);
						context.Include("includes/debug/Debug.php", true);
						context.Include("includes/Sanitizer.php", true);
						context.Include("includes/Title.php", true);
						context.Include("includes/Exception.php", true);
						context.Include("includes/User.php", true);
						context.Include("includes/StubObject.php", true);
						context.Include("includes/Hooks.php", true);
            
						context.Include("includes/GlobalFunctions.php", true);
						context.Include("includes/parser/ParserOptions.php", true);
						context.Include("includes/parser/Parser.php", true);
						context.Include("maintenance/parse.php", true);

						//context.Include("languages/messages/MessagesHy.php", true);
						//context.Include("languages/classes/LanguageHy.php", true);
			*/

			//var Title_newFromText = new PhpCallback("Title", "newFromText", context);
			//var title = Title_newFromText.Invoke(null, new object[] { "Test Page" });
			var title = (PhpObject)context.NewObject("Title");

			var lang = (PhpObject)context.NewObject("Language");

			var parserOptions = (PhpObject)context.NewObject("ParserOptions", lang, lang);

			var p = (PhpObject)context.NewObject("Parser");
			var pars = new PhpCallback(p, "parse");
			var html = pars.Invoke(null, new object[] { "''[[foo]]''", title, parserOptions });




			var parser = (PhpObject)context.NewObject("CLIParser");
			var render = new PhpCallback(parser, "render");
			var ret = render.Invoke("''[[foo]]''");
			return;

			// include the Library.php script, which initializes the 
			// whole library (it is also possible to include more 
			// scripts if necessary by repeating this call with various 
			// relative script paths):
			context.Include("Library.php", true);

			// call function f():
			context.Call("f");

			// create an instance of type C, passes array 
			// ("a" => 1, "b" => 2) as an argument to the C's ctor:
			var c = (PhpObject)context.NewObject("C", PhpArray.Keyed("a", 1, "b", 2));

			// var_dump the object:
			PhpVariable.Dump(c);

			// call static method
			var foo = new PhpCallback("C", "foo", context);
			var ret1 = foo.Invoke(null, new object[] {/*arg1, arg2, ...*/});

			// call instance method
			var bar = new PhpCallback(c, "bar"); // PhpCallback of instance method
			var ret2 = bar.Invoke(null, new object[] {/*arg1, arg2, ...*/});
		}
	}
}
