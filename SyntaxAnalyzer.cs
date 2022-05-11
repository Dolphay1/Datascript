using DataScript.StatementTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataScript
{

	static class SyntaxAnalyzer
	{
		private static readonly string[] declarationWords = new string[] { "var", "const", "function", "class", "let", "delete", "if", "while", "for" };
		private static readonly string[] reservedWords = new string[] { "var", "const", "function", "class", "break", "case", "catch", "continue", "debugger", "default", "delete", "do", "else", "enum", "export", "extends", "false", "finally", "for", "if", "import", "new", "null", "return", "super", "switch", "this", "throw", "true", "try", "typeof", "var", "void", "while", "with" };

		private static readonly Regex validVariableName = new Regex(@"^[A-Za-z][A-Za-z0-9]*&");

		private static int index = 0;
		private static int lineNumber = 0;
		public static StatementHolder ParseTree(List<string> data)
		{
			StatementHolder statements = new StatementHolder(0, "");

			parseStatementHolder(data, statements, false, new Dictionary<string, Statement>());

			return statements;
		}

		private static void parseStatementHolder(List<string> data, StatementHolder statements, bool singleStatement, Dictionary<string, Statement> activeScope)
		{
			bool endStatementHolder = false;

			
			while(!endStatementHolder)
			{

				if(Array.IndexOf(declarationWords, data[index]) > -1)
				{
					switch(data[index])
					{
						case "var":
						case "const":
						case "let":
							parseVariableStatement(data, statements);
							break;
						case "function":
							break;
						case "class":
							break;

					}

					if(singleStatement) endStatementHolder = true;
				}

				nextToken(data);
			}
		}

		private static void parseVariableStatement(List<string> data, StatementHolder statements)
		{
			bool blockScoped = (data[index] == "let" || data[index] == "const");
			string variableName;
			string variableType;

			nextToken(data);

			if (!validVariableName.IsMatch(data[index])) throw new Exception("Variable declaration expected at line "+lineNumber);
			if (Array.IndexOf(reservedWords, data[index]) > -1) throw new Exception("'" + data[index] + "' is not allowed on a variable declaration name at line "+lineNumber );

			variableName = data[index];

			nextToken(data);

			if(data[index] == ":")
			{
				nextToken(data);
				
				variableType = data[index];
				
				nextToken(data);
			}
			else
			{
				variableType = "any";
 			}

			if(data[index] == "=")
			{
				nextToken(data);

				if(data[index] == "new")
				{
					parseNew(data);
				}
			}
			
		}

		private static void parseNew(List<string> data)
		{

		}

		private static void nextToken(List<string> data) 
		{
			while(data[index] == "\n" || data[index] == "\r")
			{
				lineNumber++;
				index++;
			}
		}
	}
}
