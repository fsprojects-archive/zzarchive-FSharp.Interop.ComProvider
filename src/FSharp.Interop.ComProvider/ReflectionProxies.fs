module private FSharp.Interop.ComProvider.ReflectionProxies

open System.Reflection

type AssemblyProxy(x:Assembly) =
    inherit Assembly()
    override __.CodeBase = x.CodeBase
    override __.CreateInstance(typeName, ignoreCase, bindingAttr, binder, args, culture, activatorAttributes) = x.CreateInstance(typeName, ignoreCase, bindingAttr, binder, args, culture, activatorAttributes)
    override __.EntryPoint = x.EntryPoint
    override __.EscapedCodeBase = x.EscapedCodeBase
    override __.Evidence = x.Evidence
    override __.FullName = x.FullName
    override __.Equals(o) = x.Equals(o)
    override __.GetCustomAttributes(attributeType, inherited) = x.GetCustomAttributes(attributeType, inherited)
    override __.GetCustomAttributes(inherited) = x.GetCustomAttributes(inherited)
    override __.GetCustomAttributesData() = x.GetCustomAttributesData()
    override __.GetExportedTypes() = x.GetExportedTypes()
    override __.GetFile(name) = x.GetFile(name)
    override __.GetFiles() = x.GetFiles()
    override __.GetFiles(getResourceModules) = x.GetFiles(getResourceModules)
    override __.GetHashCode() = x.GetHashCode()
    override __.GetManifestResourceInfo(resourceName) = x.GetManifestResourceInfo(resourceName)
    override __.GetManifestResourceNames() = x.GetManifestResourceNames()
    override __.GetManifestResourceStream(ty, name) = x.GetManifestResourceStream(ty, name)
    override __.GetManifestResourceStream(name) = x.GetManifestResourceStream(name)
    override __.GetModule(name) = x.GetModule(name)
    override __.GetModules(getResourceModules) = x.GetModules(getResourceModules)
    override __.GetName() = x.GetName()
    override __.GetName(copiedName) = x.GetName(copiedName)
    override __.GetObjectData(info, context) = x.GetObjectData(info, context)
    override __.GetReferencedAssemblies() = x.GetReferencedAssemblies()
    override __.GetSatelliteAssembly(culture) = x.GetSatelliteAssembly(culture)
    override __.GetSatelliteAssembly(culture, version) = x.GetSatelliteAssembly(culture, version)
    override __.GetType(name, throwOnError, ignoreCase) = x.GetType(name, throwOnError, ignoreCase)
    override __.GetType(name, throwOnError) = x.GetType(name, throwOnError)
    override __.GetType(name) = x.GetType(name)
    override __.GetTypes() = x.GetTypes()
    override __.GlobalAssemblyCache = x.GlobalAssemblyCache
    override __.HostContext = x.HostContext
    override __.ImageRuntimeVersion = x.ImageRuntimeVersion
    override __.IsDefined(attributeType, inherited) = x.IsDefined(attributeType, inherited)
    override __.IsDynamic = x.IsDynamic
    override __.LoadModule(moduleName, rawModule, rawSymbolStore) = x.LoadModule(moduleName, rawModule, rawSymbolStore)
    override __.Location = x.Location
    override __.ManifestModule = x.ManifestModule
    override __.add_ModuleResolve(value) = x.add_ModuleResolve(value)
    override __.remove_ModuleResolve(value) = __.remove_ModuleResolve(value)
    override __.PermissionSet = x.PermissionSet
    override __.ReflectionOnly = x.ReflectionOnly
    override __.SecurityRuleSet = x.SecurityRuleSet
    override __.ToString() = x.ToString()

type MethodInfoProxy(x:MethodInfo) =
    inherit MethodInfo()
    override __.Attributes = x.Attributes
    override __.CallingConvention = x.CallingConvention
    override __.ContainsGenericParameters = x.ContainsGenericParameters
    override __.DeclaringType = x.DeclaringType
    override __.Equals(obj) = x.Equals(obj)
    override __.GetBaseDefinition() = x.GetBaseDefinition()
    override __.GetCustomAttributes(inherited) = x.GetCustomAttributes(inherited)
    override __.GetCustomAttributes(attributeType, inherited) = x.GetCustomAttributes(attributeType, inherited)
    override __.GetCustomAttributesData() = x.GetCustomAttributesData()
    override __.GetGenericArguments() = x.GetGenericArguments()
    override __.GetGenericMethodDefinition() = x.GetGenericMethodDefinition()
    override __.GetHashCode() = x.GetHashCode()
    override __.GetMethodBody() = x.GetMethodBody()
    override __.GetMethodImplementationFlags() = x.GetMethodImplementationFlags()
    override __.GetParameters() = x.GetParameters()
    override __.Invoke(obj, invokeAttr, binder, parameters, culture) = x.Invoke(obj, invokeAttr, binder, parameters, culture)
    override __.IsDefined(attributeType, inherited) = x.IsDefined(attributeType, inherited)
    override __.IsGenericMethod = x.IsGenericMethod
    override __.IsGenericMethodDefinition = x.IsGenericMethodDefinition
    override __.MetadataToken = x.MetadataToken
    override __.MethodHandle = x.MethodHandle
    override __.Module = x.Module
    override __.Name = x.Name
    override __.ReflectedType = x.ReflectedType
    override __.ReturnParameter = x.ReturnParameter
    override __.ReturnType = x.ReturnType
    override __.ReturnTypeCustomAttributes = x.ReturnTypeCustomAttributes
    override __.ToString() = x.ToString()

type PropertyInfoProxy(x:PropertyInfo) =
    inherit PropertyInfo()
    override __.Attributes = x.Attributes
    override __.CanRead = x.CanRead
    override __.CanWrite = x.CanWrite
    override __.DeclaringType = x.DeclaringType
    override __.Equals(obj) = x.Equals(obj)
    override __.GetAccessors(nonPublic) = x.GetAccessors(nonPublic)
    override __.GetConstantValue() = x.GetConstantValue()
    override __.GetCustomAttributes(inherited) = x.GetCustomAttributes(inherited)
    override __.GetCustomAttributes(attributeType, inherited) = x.GetCustomAttributes(attributeType, inherited)
    override __.GetCustomAttributesData() = x.GetCustomAttributesData()
    override __.GetGetMethod(nonPublic) = x.GetGetMethod(nonPublic)
    override __.GetHashCode() = x.GetHashCode()
    override __.GetIndexParameters() = x.GetIndexParameters()
    override __.GetOptionalCustomModifiers() = x.GetOptionalCustomModifiers()
    override __.GetRawConstantValue() = x.GetRawConstantValue()
    override __.GetRequiredCustomModifiers() = x.GetRequiredCustomModifiers()
    override __.GetSetMethod(nonPublic) = x.GetSetMethod(nonPublic)
    override __.GetValue(obj, invokeAttr, binder, index, culture) = x.GetValue(obj, invokeAttr, binder, index, culture)
    override __.IsDefined(attributeType, inherited) = x.IsDefined(attributeType, inherited)
    override __.MetadataToken = x.MetadataToken
    override __.Module = x.Module
    override __.Name = x.Name
    override __.PropertyType = x.PropertyType
    override __.ReflectedType = x.ReflectedType
    override __.SetValue(obj, value, invokeAttr, binder, index, culture) = x.SetValue(obj, value, invokeAttr, binder, index, culture)

type TypeProxy(ty) =
    inherit TypeDelegator(ty)
    override __.GetCustomAttributesData() = ty.GetCustomAttributesData()
