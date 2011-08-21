namespace MediaWiki.Net.Test
{
    using System;
    using System.Collections.Generic;

    using PHP.Core;

    public class Class1
    {
        public static void Main()
        {
            ScriptContext context = ScriptContext.CurrentContext;

            // redirect PHP output to the console:
            context.Output = Console.Out; // Unicode text output
            context.OutputStream = Console.OpenStandardOutput(); // byte stream output

            context.Constants.Add("MEDIAWIKI", null, true);

            // include the php scripts, which initializes the 
            // whole library (it is also possible to include more 
            // scripts if necessary by repeating this call with various 
            // relative script paths):
            //context.Include("MediaWiki/index.php", true);
            context.Include("MediaWiki/Languages/Language.php", true);
            context.Include("MediaWiki/includes/parser/Parser.php", true);

            foreach (KeyValuePair<IntStringKey, object> pair in context.GlobalVariables)
            {
                Console.WriteLine(pair);
            }

            var parser = (PhpObject)context.NewObject("Parser", PhpArray.Keyed("a", 1));
            var parse = new PhpCallback(parser, "parse");
            parse.Invoke();



            var getLanguageNames = new PhpCallback("Language", "getLanguageNames", context);
            getLanguageNames.Invoke();

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
