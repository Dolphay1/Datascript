using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScript.StatementTypes
{
	class Statement
	{
		private int lineNumber;
		private string token;

		public Statement(int lineNumber, string token)
		{
			this.lineNumber = lineNumber;
			this.token = token;
		}

		public int getLineNumber()
		{
			return this.lineNumber;
		}

		public string getToken()
		{
			return this.token;
		}
	}
}
