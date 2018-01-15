using System;
using System.Reflection;

namespace MedWorth.ContractManagement.Unit.Tests
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	// ReSharper disable once ClassNeverInstantiated.Global
    // No need to Instantiated
	public class Helper
	{
        /// <summary>
        /// Prevents a default instance of the <see cref="Helper"/> class from being created.
        /// </summary>
		private Helper()
		{

		}

		#region Run Method

        ///// <summary>
        /////		Runs a method on a type, given its parameters. This is useful for
        /////		calling private methods.
        ///// </summary>
        ///// <param name="t"></param>
        ///// <param name="strMethod"></param>
        ///// <param name="aobjParams"></param>
        ///// <returns>The return value of the called method.</returns>
        //public static object RunStaticMethod(Type t, string strMethod, object [] aobjParams)
        //{
        //    const BindingFlags eFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        //    return RunMethod(t, strMethod, null, aobjParams, eFlags);
        //}

	    public static object RunInstanceMethod(Type t, string strMethod, object objInstance, object [] aobjParams)
	    {
	        const BindingFlags eFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
	        return RunMethod(t, strMethod, objInstance, aobjParams, eFlags);
	    }

	    private static object RunMethod(Type t, string strMethod, object objInstance, object [] aobjParams, BindingFlags eFlags) 
		{
	        MethodInfo m = t.GetMethod(strMethod, eFlags);
		    if (m == null)
		    {
		        throw new ArgumentException("There is no method '" + strMethod + "' for type '" + t + "'.");
		    }

		    object objRet = m.Invoke(objInstance, aobjParams);
		    return objRet;
		} 

		#endregion

	} 

} 
