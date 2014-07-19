// Authors:
//   Rafael Mizrahi   <rafim@mainsoft.com>
//   Erez Lotan       <erezl@mainsoft.com>
//   Oren Gurfinkel   <oreng@mainsoft.com>
//   Ofer Borstein
// 
// Copyright (c) 2004 Mainsoft Co.
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Data;
using MonoTests.System.Data.Utils;
#if WINDOWS_STORE_APP
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using SetUpAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestInitializeAttribute;
using TearDownAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestCleanupAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using CategoryAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestCategoryAttribute;
using AssertionException = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.UnitTestAssertException;
#else
using NUnit.Framework;
#endif

namespace MonoTests_System.Data
{
	[TestFixture]
	public class VersionNotFoundExceptionTest
	{
		[Test]
		public void Generate1()
		{
			DataTable tbl = DataProvider.CreateChildDataTable();
			DataRow drParent = tbl.Rows[0];
			drParent.Delete();
			tbl.AcceptChanges();
	        
			AssertHelpers.AssertThrowsException<VersionNotFoundException>(() => {
			object obj = drParent[0,DataRowVersion.Proposed];
			});
		}

		[Test]
		public void Generate2()
		{
			DataTable tbl = DataProvider.CreateChildDataTable();
			DataRow drParent = tbl.Rows[0];
			drParent.Delete();
			tbl.AcceptChanges();
	        
			AssertHelpers.AssertThrowsException<VersionNotFoundException>(() => {
			object obj = drParent[0,DataRowVersion.Current];
			});
		}

		[Test]
		public void Generate3()
		{
			DataTable tbl = DataProvider.CreateChildDataTable();
			DataRow drParent = tbl.Rows[0];
			drParent.Delete();
			tbl.AcceptChanges();
	        
			AssertHelpers.AssertThrowsException<VersionNotFoundException>(() => {
			object obj = drParent[0,DataRowVersion.Original];
			});
		}
	}
}