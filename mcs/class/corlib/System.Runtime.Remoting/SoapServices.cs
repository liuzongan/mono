//
// System.Runtime.Remoting.SoapServices.cs
//
// Author: Jaime Anguiano Olarra (jaime@gnome.org)
//         Lluis Sanchez Gual (lluis@ximian.com)
//
// (c) 2002, Jaime Anguiano Olarra
// 


using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Metadata;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting {

	public class SoapServices
	{
		static Hashtable _xmlTypes = new Hashtable ();
		static Hashtable _xmlElements = new Hashtable ();
		static Hashtable _soapActions = new Hashtable ();
		static Hashtable _soapActionsMethods = new Hashtable ();
		static Hashtable _typeInfos = new Hashtable ();
		
		class TypeInfo
		{
			public Hashtable Attributes;
			public Hashtable Elements;
		}
		
		// Private constructor: nobody instantiates this class
		private SoapServices () {}
		
		// properties
	
		public static string XmlNsForClrType 
		{ 
			get { return "http://schemas.microsoft.com/clr/"; }
		}
		
		public static string XmlNsForClrTypeWithAssembly 
		{ 	
			get { return "http://schemas.microsoft.com/clr/assem/"; }
		}

		public static string XmlNsForClrTypeWithNs 
		{
			get { return "http://schemas.microsoft.com/clr/ns/"; }
		}

		public static string XmlNsForClrTypeWithNsAndAssembly
		{
			get { return "http://schemas.microsoft.com/clr/nsassem/"; }
		}

		
		// public methods

		public static string CodeXmlNamespaceForClrTypeNamespace (string typeNamespace, 
									string assemblyName) 
		{
			// If assemblyName is empty, then use the corlib namespace

			if (assemblyName == string.Empty)
				return XmlNsForClrTypeWithNs + typeNamespace + "/" + assemblyName;
			else
				return XmlNsForClrTypeWithNsAndAssembly + typeNamespace + "/" + assemblyName;
		}

		public static bool DecodeXmlNamespaceForClrTypeNamespace (string inNamespace, 
									out string typeNamespace, 
									out string assemblyName) {

			if (inNamespace == null) throw new ArgumentNullException ("inNamespace");

			typeNamespace = null;
			assemblyName = null;

			if (inNamespace.StartsWith(XmlNsForClrTypeWithNsAndAssembly))
			{
				int typePos = XmlNsForClrTypeWithNsAndAssembly.Length;
				if (typePos >= inNamespace.Length) return false;
				int assemPos = inNamespace.IndexOf ('/', typePos+1);
				if (assemPos == -1) return false;
				typeNamespace = inNamespace.Substring (typePos, assemPos - typePos);
				assemblyName = inNamespace.Substring (assemPos+1);
				return true;
			}
			else if (inNamespace.StartsWith(XmlNsForClrTypeWithNs))
			{
				int typePos = XmlNsForClrTypeWithNs.Length;
				typeNamespace = inNamespace.Substring (typePos);
				return true;
			}
			else
				return false;
		}

		public static void GetInteropFieldTypeAndNameFromXmlAttribute (Type containingType,
										string xmlAttribute, string xmlNamespace,
										out Type type, out string name) 
		{
			TypeInfo tf = (TypeInfo) _typeInfos [containingType];
			Hashtable ht = tf != null ? tf.Attributes : null;
			GetInteropFieldInfo (ht, xmlAttribute, xmlNamespace, out type, out name);
		}

		public static void GetInteropFieldTypeAndNameFromXmlElement (Type containingType,
										string xmlElement, string xmlNamespace,
										out Type type, out string name) 
		{
			TypeInfo tf = (TypeInfo) _typeInfos [containingType];
			Hashtable ht = tf != null ? tf.Elements : null;
			GetInteropFieldInfo (ht, xmlElement, xmlNamespace, out type, out name);
		}

		public static void GetInteropFieldInfo (Hashtable fields, 
										string xmlName, string xmlNamespace,
										out Type type, out string name) 
		{
			if (fields != null)
			{
				FieldInfo field = (FieldInfo) fields [GetNameKey (xmlName, xmlNamespace)];
				if (field != null)
				{
					type = field.FieldType;
					name = field.Name;
					return;
				}
			}
			type = null;
			name = null;
		}
		
		static string GetNameKey (string name, string namspace)
		{
			if (namspace == null) return name;
			else return name + " " + namspace;
		}

		public static Type GetInteropTypeFromXmlElement (string xmlElement, string xmlNamespace) 
		{
			lock (_xmlElements.SyncRoot)
			{
				return (Type) _xmlElements [xmlElement + " " + xmlNamespace];
			}
		}
			
		public static Type GetInteropTypeFromXmlType (string xmlType, string xmlTypeNamespace) 
		{
			lock (_xmlTypes.SyncRoot)
			{
				return (Type) _xmlTypes [xmlType + " " + xmlTypeNamespace];
			}
		}

		private static string GetAssemblyName(MethodBase mb)
		{
			if (mb.DeclaringType.Assembly == typeof (object).Assembly)
				return string.Empty;
			else
				return mb.DeclaringType.Assembly.GetName().Name;
		}

		public static string GetSoapActionFromMethodBase (MethodBase mb) 
		{
			return InternalGetSoapAction (mb);
		}

		public static bool GetTypeAndMethodNameFromSoapAction (string soapAction, 
									out string typeName, 
									out string methodName) 
		{
			lock (_soapActions.SyncRoot)
			{
				MethodBase mb = (MethodBase) _soapActionsMethods [soapAction];
				if (mb != null)
				{
					typeName = mb.DeclaringType.AssemblyQualifiedName;
					methodName = mb.Name;
					return true;
				}
			}
			
			string type;
			string assembly;

			typeName = null;
			methodName = null;

			int i = soapAction.LastIndexOf ('#');
			if (i == -1) return false;

			methodName = soapAction.Substring (i+1);

			if (!DecodeXmlNamespaceForClrTypeNamespace (soapAction.Substring (0,i), out type, out assembly) )
				return false;

			if (assembly == null) 
				typeName = type + ", " + typeof (object).Assembly.GetName().Name;
			else
				typeName = type + ", " + assembly;

			return true;
		}

		public static bool GetXmlElementForInteropType (Type type, out string xmlElement, out string xmlNamespace)
		{
			SoapTypeAttribute att = (SoapTypeAttribute) InternalRemotingServices.GetCachedSoapAttribute (type);
			if (att == null || (att.XmlElementName == null && att.XmlNamespace == null))
			{
				xmlElement = null;
				xmlNamespace = null;
				return false;
			}
			
			if (att.XmlElementName != null)
				xmlElement = att.XmlElementName;
			else
				xmlElement = type.Name;
			
			if (att.XmlNamespace != null)
				xmlNamespace = att.XmlNamespace;
			else if (att.XmlTypeNamespace != null)
				xmlNamespace = att.XmlTypeNamespace;
			else
				xmlNamespace = CodeXmlNamespaceForClrTypeNamespace (type.Namespace, type.Assembly.GetName().Name);
				
			return true;
		}

		public static string GetXmlNamespaceForMethodCall (MethodBase mb) 
		{
			return CodeXmlNamespaceForClrTypeNamespace (mb.DeclaringType.Name, GetAssemblyName(mb));
		}

		public static string GetXmlNamespaceForMethodResponse (MethodBase mb) 
		{
			return CodeXmlNamespaceForClrTypeNamespace (mb.DeclaringType.Name, GetAssemblyName(mb));
		}

		public static bool GetXmlTypeForInteropType (Type type, out string xmlType, out string xmlTypeNamespace) 
		{
			SoapTypeAttribute att = (SoapTypeAttribute) InternalRemotingServices.GetCachedSoapAttribute (type);
			
			if (att == null || (att.XmlTypeName == null && att.XmlTypeNamespace == null))
			{
				xmlType = null;
				xmlTypeNamespace = null;
				return false;
			}
			
			if (att.XmlTypeName != null)
				xmlType = att.XmlTypeName;
			else
				xmlType = type.Name;
			
			if (att.XmlTypeNamespace != null)
				xmlTypeNamespace = att.XmlTypeNamespace;
			else
				xmlTypeNamespace = CodeXmlNamespaceForClrTypeNamespace (type.Namespace, type.Assembly.GetName().Name);

			return true;
		}

		public static bool IsClrTypeNamespace (string namespaceString) 
		{
			return namespaceString.StartsWith (XmlNsForClrType);
		}

		public static bool IsSoapActionValidForMethodBase (string soapAction, MethodBase mb) 
		{
			string typeName;
			string methodName;
			GetTypeAndMethodNameFromSoapAction (soapAction, out typeName, out methodName);

			if (methodName != mb.Name) return false;

			string methodClassType = mb.DeclaringType.AssemblyQualifiedName;
			return typeName == methodClassType;
		}

		public static void PreLoad (Assembly assembly) 
		{
			foreach (Type t in assembly.GetTypes ())
				PreLoad (t);
		}

		public static void PreLoad (Type type) 
		{
			string name, namspace;
			TypeInfo tf = _typeInfos [type] as TypeInfo;
			if (tf != null) return;
			
			if (GetXmlTypeForInteropType (type, out name, out namspace))
				RegisterInteropXmlType (name, namspace, type);
				
			if (GetXmlElementForInteropType (type, out name, out namspace))
				RegisterInteropXmlElement (name, namspace, type);
				
			lock (_typeInfos.SyncRoot)
			{
				tf = new TypeInfo ();
				FieldInfo[] fields = type.GetFields (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				
				foreach (FieldInfo field in fields)
				{
					SoapFieldAttribute att = (SoapFieldAttribute) Attribute.GetCustomAttribute (field, typeof (SoapFieldAttribute));
					if (att == null) continue;
					
					string key = GetNameKey (att.XmlElementName, att.XmlNamespace);
					if (att.UseAttribute)
					{
						if (tf.Attributes == null) tf.Attributes = new Hashtable ();
						tf.Attributes [key] = field;
					}
					else
					{
						if (tf.Elements == null) tf.Elements = new Hashtable ();
						tf.Elements [key] = field;
					}
				}
				_typeInfos [type] = tf;
			}			
		}
		
		public static void RegisterInteropXmlElement (string xmlElement, string xmlNamespace, Type type) 
		{
			lock (_xmlElements.SyncRoot)
			{
				_xmlElements [xmlElement + " " + xmlNamespace] = type;
			}
		}

		public static void RegisterInteropXmlType (string xmlType, string xmlTypeNamespace, Type type) 
		{
			lock (_xmlTypes.SyncRoot)
			{
				_xmlTypes [xmlType + " " + xmlTypeNamespace] = type;
			}
		}

		public static void RegisterSoapActionForMethodBase (MethodBase mb) 
		{
			InternalGetSoapAction (mb);
		}
		
		public static string InternalGetSoapAction (MethodBase mb)
		{
			lock (_soapActions.SyncRoot)
			{
				string action = (string) _soapActions [mb];
				if (action == null)
				{
					SoapMethodAttribute att = (SoapMethodAttribute) Attribute.GetCustomAttribute (mb, typeof (SoapMethodAttribute));
					if (att != null)
						action = att.SoapAction;
					
					if (action == null)
					{
						string ns = CodeXmlNamespaceForClrTypeNamespace (mb.DeclaringType.FullName, GetAssemblyName(mb));
						action = ns + "#" + mb.Name;
					}
					_soapActions [mb] = action;
					_soapActionsMethods [action] = mb;
				}
				return action;
			}
		}

		public static void RegisterSoapActionForMethodBase (MethodBase mb, string soapAction) 
		{
			lock (_soapActions.SyncRoot)
			{
				_soapActions [mb] = soapAction;
				_soapActionsMethods [soapAction] = mb;
			}
		}
	}
}

