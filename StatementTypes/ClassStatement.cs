using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScript.StatementTypes
{
	class ClassStatement : Statement
	{
		private Dictionary<string, VariableStatement> variables;
		private Dictionary<FunctionInformation, FunctionStatement> functions;
		private Dictionary<FunctionInformation, FunctionStatement> constructors;
		public bool extends;
		public string extendor;

		public ClassStatement(int lineNumber, string token) : this(lineNumber, token, false, "")
		{
		}

		public ClassStatement(int lineNumber, string token, bool extends, string extendor) : base(lineNumber, token)
		{
			this.extends = extends;
			this.extendor = extendor;

			this.variables = new Dictionary<string, VariableStatement>();
			this.functions = new Dictionary<FunctionInformation, FunctionStatement>();
			this.constructors = new Dictionary<FunctionInformation, FunctionStatement>();
		}
	}
}
