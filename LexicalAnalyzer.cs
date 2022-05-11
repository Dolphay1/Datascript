using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataScript
{

	class LexicalAnalyzer
	{
		private static readonly Regex whitespace = new Regex(@"[ \t\n\r]");
		private static readonly Regex characters = new Regex(@"[_A-Za-z0-9]");
		public static List<string> ScanData(string data)
		{
			List<string> Tokens = new List<string>();
			char[] splitData = data.ToCharArray();
			string token = "";
			bool inString = false;
			bool inComment = false;
			bool inBlockComment = false;
			int line = 0;

			for (int i = 0; i < splitData.Length; i++)
			{
				string character = splitData[i].ToString();

				if (inString)
				{
					if (character == "\"")
					{
						try
						{
							token = Regex.Unescape(token);

							token = "\"" + token;

							token += "\"";

							Tokens.Add(token);

							inString = false;
						}
						catch
						{
						}
					}

					if (character == "\n" || character == "\r")
					{
						throw new Exception("Expected \" at line " + line);
					}
				}
				else if (inComment)
				{
					if (character == "\n" || character == "\r") inComment = false;
				}
				else if (inBlockComment)
				{
					if (character == "*" && splitData[i + 1].ToString() == "/") { inBlockComment = false; i++; };
				}
				else if (whitespace.IsMatch(character))
				{
					if (token.Length > 0)
					{
						Tokens.Add(token);
						token = "";
					}

					if (character == "\n")
					{
						line++;
						Tokens.Add(character);
					}
				}
				else if (characters.IsMatch(character))
				{
					token = token + character;
				}
				else
				{
					if (token.Length > 0)
					{
						Tokens.Add(token);
						token = "";
					}

					if (character == "\"") inString = true;

					else if(character == "/" && splitData[i + 1].ToString() == "/") { inComment = true; i++; }

					else if (character == "/" && splitData[i + 1].ToString() == "*") { inBlockComment = true; i++; }

					if(!inComment && !inBlockComment) Tokens.Add(character);
				}


			}

			if (inString)
			{
				throw new Exception("Expected closing \" at line " + line);
			}

			return Tokens;
		}
	}
}
