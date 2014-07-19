// Authors:
//   Nagappan A <anagappan@novell.com>
//
// Copyright (c) 2007 Novell, Inc
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

#if NET_2_0
using System;
using System.Data;
using System.Collections;
using System.IO;	
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
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

#if WINDOWS_STORE_APP
using System.Xml.Linq;
using XmlNode = System.Xml.Linq.XNode;
#endif

namespace Monotests_System.Data
{
	[TestFixture]	
	public class XmlDataReaderTest
	{
		[Test]
		public void XmlLoadTest ()
		{
				DataSet ds = new DataSet();
				ds.ReadXmlSchema ("Test/System.Data/TestReadXmlSchema1.xml");
				ds.ReadXml ("Test/System.Data/TestReadXml1.xml");
		}
		
		// Test for Bug#377146
		[Test]
		public void XmlLoadCustomTypesTest ()
		{
			string xml = "<CustomTypesData>" + Environment.NewLine +
						"<CustomTypesTable>" + Environment.NewLine +
    					"<Dummy>99</Dummy>" + Environment.NewLine +
						"<FuncXml> " + Environment.NewLine +
						"<Func Name=\"CUT_IntPassiveIn()\" Direction=\"PASSIVE_MOCK\">" + Environment.NewLine +
						"<Param Name=\"paramLen\" Type=\"int\" Len=\"1\" InOut=\"IN\" Union=\"FALSE\" " + Environment.NewLine +
						"Callback=\"\" CSharpType=\"int\" Value=\"\" ExpectedValue=\"1\" IsExpGetRef=\"\" " + Environment.NewLine +
						"IsGetRef=\"\" IsSetRef=\"\" ChildSelected=\"FALSE\" UnionIndex=\"-1\" HandleInput=\"DEC\" " + Environment.NewLine +
						"Enum=\"\">" + Environment.NewLine + 
						"</Param>" + Environment.NewLine + Environment.NewLine +
						"<Param Name=\"single\" Type=\"int\" Len=\"1\" InOut=\"IN\" Union=\"FALSE\" " + Environment.NewLine +
						"Callback=\"\" CSharpType=\"int\" Value=\"\" ExpectedValue=\"16\" IsExpGetRef=\"\" " + Environment.NewLine +
						"IsGetRef=\"\" IsSetRef=\"\" ChildSelected=\"FALSE\" UnionIndex=\"-1\" HandleInput=\"DEC\" " + Environment.NewLine +
						"Enum=\"\">" + Environment.NewLine + 
						"</Param>" + Environment.NewLine + Environment.NewLine +
						"<Param Name=\"arraySizeParam\" Type=\"int*\" Len=\"4\" InOut=\"IN\" " + Environment.NewLine +
						"Union=\"FALSE\" Callback=\"\" CSharpType=\"int\" Value=\"\" ExpectedValue=\"\" " + Environment.NewLine +
						"IsExpGetRef=\"\" IsGetRef=\"\" IsSetRef=\"\" ChildSelected=\"FALSE\" UnionIndex=\"-1\" " + Environment.NewLine +
						"HandleInput=\"HEX\" Enum=\"\">" + Environment.NewLine + Environment.NewLine +
						"<Param1 Name=\"arraySizeParam0\" Type=\"int\" Len=\"0\" InOut=\"IN\" " + Environment.NewLine +
						"Union=\"FALSE\" Callback=\"\" CSharpType=\"int\" Value=\"\" ExpectedValue=\"1\" " + Environment.NewLine +
						"IsExpGetRef=\"\" IsGetRef=\"\" IsSetRef=\"\" ChildSelected=\"FALSE\" UnionIndex=\"-1\" " + Environment.NewLine +
						"HandleInput=\"DEC\" Enum=\"\">" + Environment.NewLine +
						"</Param1>" + Environment.NewLine + Environment.NewLine +
						"<Param1 Name=\"arraySizeParam1\" Type=\"int\" Len=\"0\" InOut=\"IN\" " + Environment.NewLine +
						"Union=\"FALSE\" Callback=\"\" CSharpType=\"int\" Value=\"\" ExpectedValue=\"\" " + Environment.NewLine +
						"IsExpGetRef=\"\" IsGetRef=\"\" IsSetRef=\"\" ChildSelected=\"FALSE\" UnionIndex=\"-1\" " + Environment.NewLine +
						"HandleInput=\"HEX\" Enum=\"\">" + Environment.NewLine +
						"</Param1>" + Environment.NewLine + Environment.NewLine +
						"<Param1 Name=\"arraySizeParam2\" Type=\"int\" Len=\"0\" InOut=\"IN\" " + Environment.NewLine +
						"Union=\"FALSE\" Callback=\"\" CSharpType=\"int\" Value=\"\" ExpectedValue=\"\" " + Environment.NewLine +
						"IsExpGetRef=\"\" IsGetRef=\"\" IsSetRef=\"\" ChildSelected=\"FALSE\" UnionIndex=\"-1\" " + Environment.NewLine +
						"HandleInput=\"HEX\" Enum=\"\">" + Environment.NewLine +
						"</Param1>" + Environment.NewLine + Environment.NewLine +
						"<Param1 Name=\"arraySizeParam3\" Type=\"int\" Len=\"0\" InOut=\"IN\" " + Environment.NewLine +
						"Union=\"FALSE\" Callback=\"\" CSharpType=\"int\" Value=\"\" ExpectedValue=\"\" " + Environment.NewLine +
						"IsExpGetRef=\"\" IsGetRef=\"\" IsSetRef=\"\" ChildSelected=\"FALSE\" UnionIndex=\"-1\" " + Environment.NewLine +
						"HandleInput=\"HEX\" Enum=\"\">" + Environment.NewLine +
						"</Param1>" + Environment.NewLine + Environment.NewLine +
        				"</Param>" + Environment.NewLine +
						"<Return Name=\"retVal\" Type=\"int\" Len=\"1\" InOut=\"OUT\" Union=\"FALSE\" " + Environment.NewLine +
						"Callback=\"\" CSharpType=\"int\" Value=\"1\" ExpectedValue=\"\" IsExpGetRef=\"\" " + Environment.NewLine +
						"IsGetRef=\"\" IsSetRef=\"\" ChildSelected=\"FALSE\" UnionIndex=\"-1\" HandleInput=\"DEC\" " + Environment.NewLine +
						"Enum=\"\">" + Environment.NewLine +
						"</Return>" + Environment.NewLine +
						"</Func>" + Environment.NewLine +
						"</FuncXml>" + Environment.NewLine +
						"</CustomTypesTable>" + Environment.NewLine +
						"</CustomTypesData>" + Environment.NewLine;

			DataTable tbl = new DataTable("CustomTypesTable");
			tbl.Columns.Add("Dummy", typeof(System.UInt32));
			tbl.Columns.Add("FuncXml", typeof(CustomTypeXml));

			DataSet ds = new DataSet("CustomTypesData");
			ds.Tables.Add(tbl);

			using (StringReader sr = new StringReader (xml))
			using (XmlReader xr = XmlReader.Create (sr)) {
				ds.ReadXml(xr);
			}

			Assert.AreEqual (1, ds.Tables["CustomTypesTable"].Rows.Count, "XDR2");
			Assert.AreEqual (99, Convert.ToInt32(ds.Tables["CustomTypesTable"].Rows[0][0]), "XDR3");
			Assert.IsTrue (ds.Tables["CustomTypesTable"].Rows[0][1].ToString().StartsWith("<Func "), "XDR4");
		}
		
#if !WINDOWS_STORE_APP
		[Serializable]
#endif
		public class CustomTypeXml : IXmlSerializable {
		    XmlNode mFuncXmlNode;

		    #region Constructors
		    public CustomTypeXml()
		    {
		    }

		    public CustomTypeXml(string str)
		    {
#if !WINDOWS_STORE_APP
		        XmlDocument doc = new XmlDocument ();
		        doc.LoadXml (str);
		        mFuncXmlNode = (XmlNode)(doc.DocumentElement);
#else
		        XDocument doc = XDocument.Parse (str);
		        mFuncXmlNode = doc.Root;
#endif
		    }

		    public CustomTypeXml (XmlNode xNode)
		    {
		        mFuncXmlNode = xNode;
		    }
		    #endregion

		    #region Node (set/get)
		    public XmlNode Node {
		        get { return mFuncXmlNode; }
		        set { this.mFuncXmlNode = value; }
		    }
		    #endregion
		    #region ToString
		    public override string ToString ()
		    {
#if !WINDOWS_STORE_APP
		        return this.Node.OuterXml;
#else
				return this.Node.ToString ();
#endif
		    }
		    #endregion

		    /* IXmlSerializable overrides */
		    #region WriteXml
		    void IXmlSerializable.WriteXml (XmlWriter writer)
		    {
#if !WINDOWS_STORE_APP
		        XmlDocument doc = new XmlDocument ();
		        doc.LoadXml (mFuncXmlNode.OuterXml);

		        // On function level
		        if (doc.DocumentElement.Name == "Func") {
		            try {
                        doc.DocumentElement.Attributes.Remove (doc.DocumentElement.Attributes ["ReturnType"]);
		            } catch { 
		            }
		            try { 
		                doc.DocumentElement.Attributes.Remove (doc.DocumentElement.Attributes ["ReturnTId"]);
		            } catch {
		            }
		            try { 
		                doc.DocumentElement.Attributes.Remove (doc.DocumentElement.Attributes ["CSharpType"]); 
		            } catch {
		            }
		        } else {
		            UpgradeSchema (doc.DocumentElement);
		        }

		        // Make sure lrt is saved according to latest schema
		        foreach (XmlNode n in doc.DocumentElement.ChildNodes) {
		            UpgradeSchema (n);
		        }
#else
				XDocument doc = XDocument.Parse(mFuncXmlNode.ToString());

		        // On function level
		        if (doc.Root.Name.LocalName == "Func")
		        {
		            try { 
						doc.Root.Attribute ("ReturnType").Remove (); 
					} catch { 
					}
		            try { 
						doc.Root.Attribute ("ReturnTId").Remove (); 
					} catch {
					}
					try {
						doc.Root.Attribute ("CSharpType").Remove ();
					} catch {
					}
		        } else {
		            UpgradeSchema (doc.Root);
		        }

		        // Make sure lrt is saved according to latest schema
		        foreach (XElement e in doc.Root.Elements ()) {
		            UpgradeSchema (e);
		        }
#endif

		        doc.WriteTo (writer);
		    }
		    #endregion
		    #region ReadXml
		    void IXmlSerializable.ReadXml (XmlReader reader)
		    {
#if !WINDOWS_STORE_APP
		        XmlDocument doc = new XmlDocument ();
		        string str = reader.ReadString ();
		        try {
		            doc.LoadXml (str);
		        } catch {
		            doc.LoadXml (reader.ReadOuterXml ());
		        }
		        mFuncXmlNode = (XmlNode)(doc.DocumentElement);
#else
				string str = reader.ReadInnerXml ();
				mFuncXmlNode = XDocument.Parse (str).Root;
#endif
		    }
		    #endregion
		    #region GetSchema
		    XmlSchema IXmlSerializable.GetSchema ()
		    {
		        return (null);
		    }
		    #endregion

		    /* Private utils */
		    #region private UpgradeSchema
#if !WINDOWS_STORE_APP
		    void UpgradeSchema (XmlNode xNode)
		    {
		        // Attribute removals (cleanup)
		        try { 
			        xNode.Attributes.Remove (xNode.Attributes ["TId"]);
			    } catch {
			    }
		        try { 
			        xNode.Attributes.Remove (xNode.Attributes ["OnError"]);
			    } catch {
			    }
		        try {
			        xNode.Attributes.Remove (xNode.Attributes ["Check"]);
			    } catch {
			    }
		        try {
			        xNode.Attributes.Remove (xNode.Attributes ["ParamType"]);
			    } catch {
			    }
		        try {
			        xNode.Attributes.Remove (xNode.Attributes ["RealLen"]);
			    } catch {
			    }

		        // Attribute removals (order)
		        try {
		            XmlAttribute attr = xNode.Attributes ["IsExpGetRef"];
		            xNode.Attributes.Remove (xNode.Attributes ["IsExpGetRef"]);
		            xNode.Attributes.InsertAfter (attr, xNode.Attributes ["ExpectedValue"]);
			    } catch {
			    }

		        // Attribute value formats (prefix, etc.)
		        string tmp = xNode.Attributes ["HandleInput"].Value;
		        tmp = tmp.Replace ("E_LRT_INPUT_HANDLE_", "");
		        xNode.Attributes ["HandleInput"].Value = tmp;

		        foreach (XmlNode n in xNode.ChildNodes) {
		            UpgradeSchema (n);
		        }
		    }
#else
			void UpgradeSchema (XElement xNode)
		    {
		        // Attribute removals (cleanup)
		        try { 
					xNode.Attribute ("TId").Remove (); 
			    } catch {
			    }
		        try { 
					xNode.Attribute ("OnError").Remove ();
			    } catch {
			    }
		        try { 
					xNode.Attribute ("Check").Remove ();
			    } catch {
			    }
		        try { 
					xNode.Attribute ("ParamType").Remove ();
			    } catch {
			    }
				try {
					xNode.Attribute ("RealLen").Remove ();
			    } catch {
			    }

		        // Attribute removals (order)
		        try {
		            XAttribute attr = xNode.Attribute ("IsExpGetRef");
		            attr.Remove ();
		            xNode.Add (attr);
			    } catch {
			    }

		        // Attribute value formats (prefix, etc.)
		        string tmp = xNode.Attribute ("HandleInput").Value;
		        tmp = tmp.Replace ("E_LRT_INPUT_HANDLE_", "");
		        xNode.Attribute ("HandleInput").Value = tmp;

		        foreach (XElement n in xNode.Elements ()) {
		            UpgradeSchema (n);
		        }
		    }
#endif
		    #endregion
		}	
	}
}
#endif

