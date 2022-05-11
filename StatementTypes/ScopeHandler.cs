using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScript.StatementTypes
{

	/*
	 * This is to manage the scope of the files making 
	 * it as similar to normal typescript as possible.
	 * 
	 * Javascript and by extension typescript has a very weird way of scopiong
	 * 
	 * In this document I define it as 3 different types: Global, Active, and Local
	 * Although normally this would be named Global, Function, and Block scope, 
	 * for readability I changed function and block to active and local
	 * 
	 * 
	 * Global functions/classes/variables are ones that have been created above the current scope 
	 * There should never be a duplicate function or class, however variables are managed differently
	 * 
	 * Active functions/classes/variables are ones that have been created inside the current scope
	 * These will be copied to the scope outside the current scope as long as the current scope is a block scope
	 * Any duplicate functions or classes will cause an error, variables are explained below
	 * 
	 * Local variables are ones that have been created with the "let" attribute 
	 * and no matter the scope type will be removed after it is closed
	 * 
	 * 
	 * Variables created with the "var" attribute will make a variable that is kept until the current function scope is closed
	 * 
	 * Duplicate variables made with the "var" attribute will just modify the original 
	 * variable given the attempted assignee is the same type of the variable
	 * 
	 * If there is a variable in the global group (variables that have been made outside of the current scope)
	 * and a variable created with the "let" attribute in the current scope, it will just act as if a new variable is 
	 * created and not motify the original variable
	 * 
	 * If 2 variables are created in the same block scope where one uses the "let" attribute an error will occur no matter the other type
	 * 
	 * All the rules of the "let" attribute apply to the "const" attribute, however the variable cannot be modified in the current scope
	 * 
	 * This whole system is very dumb as if everything was just in the block scope you wouldnt have to deal with all of these rules.
	 */

	class ScopeHandler
	{
		/*
		 * This defines all variables/functions/classes that are above the current scope ex:
		 * 
		 * ==========================
		 * 
		 * var i = 5;
		 * if(true) {
		 *	  console.log(i);
		 * }
		 * 
		 * ==========================
		 * 
		 * The variable i would be considered global as it will not be 
		 * removed from the scope after the current scope is closed.
		 * 
		 * This applies to classes, functions, and variables.
		 * 
		 */
		private Dictionary<FunctionInformation, FunctionStatement> globalFunctions;
		private Dictionary<string, VariableStatement> globalVariables;
		private Dictionary<string, ClassStatement> globalClasses;


		/*
		 * This defines variables/functions/classes created in the current scope that will 
		 * be allowed to leave the scope if the current scope given the following is true:
		 * 
		 * The current scope is not a function scope (explained at the top of the class)
		 * The variable is set as a "var" ex:
		 * 
		 * ==========================
		 * 
		 * if(true) {
		 *	  var i = 5;
		 * }
		 * 
		 * console.log(i);
		 *
		 * ==========================
		 * 
		 * This would output 5 as the i stays in the scope even when leaving the block scope.
		 * 
		 */
		private Dictionary<FunctionInformation, FunctionStatement> activeFunctions;
		private Dictionary<string, VariableStatement> activeVariables;
		private Dictionary<string, ClassStatement> activeClasses;

		/*
		 * This defines variables that are created within the current
		 * scope however will not be allowed to leave the current scope
		 * 
		 * Variables must be set with "const" or "let" for this to be true ex:
		 * 
		 * ==========================
		 * 
		 * if(true) {
		 *	  let i = 5;
		 * }
		 * 
		 * console.log(i);
		 * 
		 * ==========================
		 * 
		 * This would output undefined as i loses value when leaving the block scope.
		 * 
		 */
		private Dictionary<string, VariableStatement> localVariables;

		public ScopeHandler()
		{
			this.globalFunctions = new Dictionary<FunctionInformation, FunctionStatement>();		// Creates the list of global Functions
			this.globalVariables = new Dictionary<string, VariableStatement>();						// Creates the list of global Variables
			this.globalClasses = new Dictionary<string, ClassStatement>();							// Creates the list of global Classes

			this.activeFunctions = new Dictionary<FunctionInformation, FunctionStatement>();		// Creates the list of active Functions
			this.activeVariables = new Dictionary<string, VariableStatement>();						// Creates the list of active Variables
			this.activeClasses = new Dictionary<string, ClassStatement>();							// Creates the list of active Classes

			this.localVariables = new Dictionary<string, VariableStatement>();						// Creates the list of local Variables
		}

		private ScopeHandler(ScopeHandler previousScope)
		{
			(globalFunctions, globalVariables, globalClasses) = previousScope.getDictionaries();	// Copies the scope handlers Functions, Variables, and Classes into the global dictionaries

			this.activeFunctions = new Dictionary<FunctionInformation, FunctionStatement>();		// Creates the list of active Functions
			this.activeVariables = new Dictionary<string, VariableStatement>();						// Creates the list of active Variables
			this.activeClasses = new Dictionary<string, ClassStatement>();							// Creates the list of active Classes

			this.localVariables = new Dictionary<string, VariableStatement>();						// Creates the list of local Variables
		}

		/*
		 * This function returns the currently active Functions, Variables, and Classes for the scope above to use, removing the local variables
		 */

		private (Dictionary<FunctionInformation, FunctionStatement>, Dictionary<string, VariableStatement>, Dictionary<string, ClassStatement>) getActive()
		{
			return (this.activeFunctions, this.activeVariables, this.activeClasses);
		}

		/*
		 * This function creates 3 dictionaries to combine the entire scope into allowing for easy transfer of everything.
		 */

		private (Dictionary<FunctionInformation, FunctionStatement>, Dictionary<string, VariableStatement>, Dictionary<string, ClassStatement>) getDictionaries()
		{
			Dictionary<FunctionInformation, FunctionStatement> combinedFunctions = new Dictionary<FunctionInformation, FunctionStatement>();
			Dictionary<string, VariableStatement> combinedVariables = new Dictionary<string, VariableStatement>();
			Dictionary<string, ClassStatement> combinedClasses = new Dictionary<string, ClassStatement>();

			globalFunctions.ToList().ForEach(x => combinedFunctions[x.Key] = x.Value);
			activeFunctions.ToList().ForEach(x => combinedFunctions[x.Key] = x.Value);

			globalVariables.ToList().ForEach(x => combinedVariables[x.Key] = x.Value);
			activeVariables.ToList().ForEach(x => combinedVariables[x.Key] = x.Value);
			localVariables.ToList().ForEach(x => combinedVariables[x.Key] = x.Value);

			globalClasses.ToList().ForEach(x => combinedClasses[x.Key] = x.Value);
			activeClasses.ToList().ForEach(x => combinedClasses[x.Key] = x.Value);

			return (combinedFunctions, combinedVariables, combinedClasses);
		}

		/*
		 * This looks for a class in both active, and global scopes
		 */

		public ClassStatement? findClass(string classToFind)
		{
			if (activeClasses.ContainsKey(classToFind))
				return activeClasses[classToFind];
			else if (globalClasses.ContainsKey(classToFind))
				return globalClasses[classToFind];
			else
				return null;
		}

		/*
		 * This looks for a variable in local, active, and global scopes
		 */

		public VariableStatement? findVariable(string variableToFind)
		{
			if (localVariables.ContainsKey(variableToFind))
				return localVariables[variableToFind];
			else if (activeVariables.ContainsKey(variableToFind))
				return activeVariables[variableToFind];
			else if (globalVariables.ContainsKey(variableToFind))
				return globalVariables[variableToFind];
			else
				return null;
		}

		/*
		 * This looks for function in local, active, and global scopes
		 */

		public FunctionStatement? findFunction(FunctionInformation functionToFind)
		{
			if (activeFunctions.ContainsKey(functionToFind))
				return activeFunctions[functionToFind];
			else if (globalFunctions.ContainsKey(functionToFind))
				return globalFunctions[functionToFind];
			else
				return null;
		}

		/*
		 * This adds a variable to either the local or active scope, if local is true it will be added to the local scope
		 */

		public void addVariable(string variableName, VariableStatement variable, bool local)
		{
			if (local) localVariables.Add(variableName, variable);
			else activeVariables.Add(variableName, variable);
		}

		/*
		 * This adds a function to the active scope
		 */

		public void addFunction(FunctionInformation functionInformation, FunctionStatement function)
		{
			activeFunctions.Add(functionInformation, function);
		}

		/*
		 * This adds a class to the active scocpe
		 */

		public void addClass(string className, ClassStatement classStatement)
		{
			activeClasses.Add(className, classStatement);
		}

		/*
		 * This creates a new scope handler with all the current functions, classes, and variables in the global scope.
		 */

		public ScopeHandler scopeDown()
		{
			return new ScopeHandler(this);
		}

		/*
		 * This takes a ScopeHandler's active variables and adds them to the current scope,
		 * however all duplicate Classes and Functions will cause an error
		 */

		public void scopeUp(ScopeHandler scopeHandler)
		{
			(Dictionary<FunctionInformation, FunctionStatement>, Dictionary<string, VariableStatement>, Dictionary<string, ClassStatement>) dictionaries = scopeHandler.getActive();

			dictionaries.Item1.ToList().ForEach(x =>
			{

				try
				{
					activeFunctions.Add(x.Key, x.Value);
				}
				catch (ArgumentException ex)
				{
					throw new Exception("Error, duplicate function named " + x.Key + " at line " + x.Value.getLineNumber());
				}

			});

			dictionaries.Item3.ToList().ForEach(x =>
			{

				try
				{
					activeClasses.Add(x.Key, x.Value);
				}
				catch (ArgumentException ex)
				{
					throw new Exception("Error, duplicate class named " + x.Key + " at line " + x.Value.getLineNumber());
				}

			});

			dictionaries.Item2.ToList().ForEach(x => {
				activeVariables.Add(x.Key, x.Value);
			});
		}
	}
}
