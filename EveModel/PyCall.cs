using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace EveModel
{
    public class PyCall
    {
        #region PInvokes
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr intptr_0, string string_0);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string string_0);
        [DllImport("python27.dll", EntryPoint = "Py_BuildValue")]
        public static extern IntPtr Py_BuildValue_Params6(string string_0, IntPtr intptr_0, IntPtr intptr_1, IntPtr intptr_2, IntPtr intptr_3, IntPtr intptr_4, IntPtr intptr_5);
        [DllImport("python27.dll", EntryPoint = "Py_BuildValue")]
        public static extern IntPtr Py_BuildValue_Params4(string string_0, IntPtr intptr_0, IntPtr intptr_1, IntPtr intptr_2, IntPtr intptr_3);
        [DllImport("python27.dll", EntryPoint = "Py_BuildValue")]
        public static extern IntPtr Py_BuildValue_Params1(string string_0, IntPtr intptr_0);
        [DllImport("python27.dll", EntryPoint = "Py_BuildValue")]
        public static extern IntPtr Py_BuildValue_Params0(string string_0);
        [DllImport("python27.dll", EntryPoint = "Py_BuildValue")]
        public static extern IntPtr Py_BuildValue_4(string string_0, IntPtr intptr_0, IntPtr intptr_1, IntPtr intptr_2, IntPtr intptr_3, IntPtr intptr_4, IntPtr intptr_5, IntPtr intptr_6);
        [DllImport("python27.dll", EntryPoint = "Py_BuildValue")]
        public static extern IntPtr Py_BuildValue_Params2(string string_0, IntPtr intptr_0, IntPtr intptr_1);
        [DllImport("python27.dll", EntryPoint = "Py_BuildValue")]
        public static extern IntPtr Py_BuildValue_Params5(string string_0, IntPtr intptr_0, IntPtr intptr_1, IntPtr intptr_2, IntPtr intptr_3, IntPtr intptr_4);
        [DllImport("python27.dll", EntryPoint = "Py_BuildValue")]
        public static extern IntPtr Py_BuildValue_Params3(string string_0, IntPtr intptr_0, IntPtr intptr_1, IntPtr intptr_2);
        [DllImport("python27.dll")]
        public static extern void Py_DecRef(IntPtr intptr_0);
        [DllImport("python27.dll")]
        public static extern void Py_IncRef(IntPtr intptr_0);
        [DllImport("python27.dll")]
        public static extern IntPtr PyBool_FromLong(int int_0);
        [DllImport("python27.dll")]
        public static extern IntPtr PyCode_NewEmpty(string string_0, string string_1, int int_0);
        [DllImport("python27.dll")]
        public static extern int PyDict_DelItem(IntPtr intptr_0, IntPtr intptr_1);
        [DllImport("python27.dll")]
        public static extern IntPtr PyDict_GetItem(IntPtr intptr_0, IntPtr intptr_1);
        [DllImport("python27.dll")]
        public static extern IntPtr PyDict_New();
        [DllImport("python27.dll")]
        public static extern int PyDict_SetItem(IntPtr intptr_0, IntPtr intptr_1, IntPtr intptr_2);
        [DllImport("python27.dll")]
        public static extern void PyErr_Clear();
        [DllImport("python27.dll")]
        public static extern IntPtr PyErr_Occurred();
        [DllImport("python27.dll")]
        public static extern IntPtr PyEval_CallObjectWithKeywords(IntPtr intptr_0, IntPtr intptr_1, IntPtr intptr_2);
        [DllImport("python27.dll")]
        public static extern double PyFloat_AsDouble(IntPtr intptr_0);
        [DllImport("python27.dll")]
        public static extern IntPtr PyFloat_FromDouble(double double_0);
        [DllImport("python27.dll")]
        public static extern IntPtr PyFrame_New(IntPtr intptr_0, IntPtr intptr_1, IntPtr intptr_2, IntPtr intptr_3);
        [DllImport("python27.dll")]
        public static extern IntPtr PyImport_ImportModule(string string_0);
        [DllImport("python27.dll")]
        public static extern IntPtr PyList_GetItem(IntPtr intptr_0, int int_0);
        [DllImport("python27.dll")]
        public static extern IntPtr PyList_New(int int_0);
        [DllImport("python27.dll")]
        public static extern int PyList_SetItem(IntPtr intptr_0, int int_0, IntPtr intptr_1);
        [DllImport("python27.dll")]
        public static extern int PyList_SetSlice(IntPtr intptr_0, int int_0, int int_1, IntPtr intptr_1);
        [DllImport("python27.dll")]
        public static extern int PyList_Size(IntPtr intptr_0);
        [DllImport("python27.dll")]
        public static extern int PyLong_AsLong(IntPtr intptr_0);
        [DllImport("python27.dll")]
        public static extern long PyLong_AsLongLong(IntPtr intptr_0);
        [DllImport("python27.dll")]
        public static extern IntPtr PyLong_FromLong(int int_0);
        [DllImport("python27.dll")]
        public static extern IntPtr PyLong_FromLongLong(long long_0);
        [DllImport("python27.dll")]
        public static extern IntPtr PyObject_GetAttrString(IntPtr intptr_0, string string_0);
        [DllImport("python27.dll")]
        public static extern int PyObject_SetAttrString(IntPtr intptr_0, string string_0, IntPtr intptr_1);
        [DllImport("python27.dll")]
        public static extern IntPtr PyString_AsString(IntPtr intptr_0);
        [DllImport("python27.dll")]
        public static extern IntPtr PyString_FromString(string string_0);
        [DllImport("python27.dll")]
        public static extern int PyString_Size(IntPtr intptr_0);
        [DllImport("python27.dll")]
        public static extern IntPtr PyTuple_GetItem(IntPtr intptr_0, int int_0);
        [DllImport("python27.dll")]
        public static extern int PyTuple_Size(IntPtr intptr_0);
        [DllImport("python27.dll")]
        public static extern IntPtr PyUnicodeUCS2_AsUnicode(IntPtr intptr_0);
        [DllImport("python27.dll")]
        public static extern int PyUnicodeUCS2_GetSize(IntPtr intptr_0);
        #endregion

        #region PythonTypes
        static bool _pythontypesLoaded;
        public static Dictionary<IntPtr, PyType?> _pythonTypes = new Dictionary<IntPtr, PyType?>();
        public static Dictionary<string, IntPtr> _loadedTypes = new Dictionary<string, IntPtr>();
        public static void PopulatePythonTypes()
        {
            _pythontypesLoaded = true;
            _pythonTypes.Add(GetTypePtr("PyBaseObject_Type"), PyType.BaseObjectType);
            _pythonTypes.Add(GetTypePtr("PyBaseString_Type"), PyType.BaseStringType);
            _pythonTypes.Add(GetTypePtr("PyBomb_Type"), PyType.BombType);
            _pythonTypes.Add(GetTypePtr("PyBool_Type"), PyType.BoolType);
            _pythonTypes.Add(GetTypePtr("PyBuffer_Type"), PyType.BufferType);
            _pythonTypes.Add(GetTypePtr("PyByteArrayIter_Type"), PyType.ByteArrayIterType);
            _pythonTypes.Add(GetTypePtr("PyByteArray_Type"), PyType.ByteArrayType);
            _pythonTypes.Add(GetTypePtr("PyCFrame_Type"), PyType.CFrameType);
            _pythonTypes.Add(GetTypePtr("PyCFunction_Type"), PyType.CFunctionType);
            _pythonTypes.Add(GetTypePtr("PyCObject_Type"), PyType.CObjectType);
            _pythonTypes.Add(GetTypePtr("PyCStack_Type"), PyType.CStackType);
            _pythonTypes.Add(GetTypePtr("PyCallIter_Type"), PyType.CallIterType);
            _pythonTypes.Add(GetTypePtr("PyCapsule_Type"), PyType.CapsuleType);
            _pythonTypes.Add(GetTypePtr("PyCell_Type"), PyType.CellType);
            _pythonTypes.Add(GetTypePtr("PyChannel_TypePtr"), PyType.ChannelTypePtr);
            _pythonTypes.Add(GetTypePtr("PyClassMethodDescr_Type"), PyType.ClassMethodDescrType);
            _pythonTypes.Add(GetTypePtr("PyClassMethod_Type"), PyType.ClassMethodType);
            _pythonTypes.Add(GetTypePtr("PyClass_Type"), PyType.ClassType);
            _pythonTypes.Add(GetTypePtr("PyCode_Type"), PyType.CodeType);
            _pythonTypes.Add(GetTypePtr("PyComplex_Type"), PyType.ComplexType);
            _pythonTypes.Add(GetTypePtr("PyDictItems_Type"), PyType.DictItemsType);
            _pythonTypes.Add(GetTypePtr("PyDictIterItem_Type"), PyType.DictIterItemType);
            _pythonTypes.Add(GetTypePtr("PyDictIterKey_Type"), PyType.DictIterKeyType);
            _pythonTypes.Add(GetTypePtr("PyDictIterValue_Type"), PyType.DictIterValueType);
            _pythonTypes.Add(GetTypePtr("PyDictKeys_Type"), PyType.DictKeysType);
            _pythonTypes.Add(GetTypePtr("PyDictProxy_Type"), PyType.DictProxyType);
            _pythonTypes.Add(GetTypePtr("PyDictValues_Type"), PyType.DictValuesType);
            _pythonTypes.Add(GetTypePtr("PyDict_Type"), PyType.DictType);
            _pythonTypes.Add(GetTypePtr("PyEllipsis_Type"), PyType.EllipsisType);
            _pythonTypes.Add(GetTypePtr("PyEnum_Type"), PyType.EnumType);
            _pythonTypes.Add(GetTypePtr("PyExc_TypeError"), PyType.ExcTypeError);
            _pythonTypes.Add(GetTypePtr("PyFile_Type"), PyType.FileType);
            _pythonTypes.Add(GetTypePtr("PyFlexType_TypePtr"), PyType.FlexTypeTypePtr);
            _pythonTypes.Add(GetTypePtr("PyFloat_Type"), PyType.FloatType);
            _pythonTypes.Add(GetTypePtr("PyFrame_Type"), PyType.FrameType);
            _pythonTypes.Add(GetTypePtr("PyFrozenSet_Type"), PyType.FrozenSetType);
            _pythonTypes.Add(GetTypePtr("PyFunction_Type"), PyType.FunctionType);
            _pythonTypes.Add(GetTypePtr("PyGen_Type"), PyType.GenType);
            _pythonTypes.Add(GetTypePtr("PyGetSetDescr_Type"), PyType.GetSetDescrType);
            _pythonTypes.Add(GetTypePtr("PyInstance_Type"), PyType.InstanceType);
            _pythonTypes.Add(GetTypePtr("PyInt_Type"), PyType.IntType);
            _pythonTypes.Add(GetTypePtr("PyList_Type"), PyType.ListType);
            _pythonTypes.Add(GetTypePtr("PyLong_Type"), PyType.LongType);
            _pythonTypes.Add(GetTypePtr("PyMemberDescr_Type"), PyType.MemberDescrType);
            _pythonTypes.Add(GetTypePtr("PyMemoryView_Type"), PyType.MemoryViewType);
            //_pythonTypes.Add(GetTypePtr("PyMethodDescr_Type"), PyType.MethodDescrType);
            //_pythonTypes.Add(GetTypePtr("PyMethodWrapper_Type"), PyType.MethodWrapperType);
            _pythonTypes.Add(GetTypePtr("PyMethod_Type"), PyType.MethodType);
            _pythonTypes.Add(GetTypePtr("PyModule_Type"), PyType.ModuleType);
            _pythonTypes.Add(GetTypePtr("PyNullImporter_Type"), PyType.NullImporterType);
            _pythonTypes.Add(GetTypePtr("PyObject_Type"), PyType.ObjectType);
            _pythonTypes.Add(GetTypePtr("PyProperty_Type"), PyType.PropertyType);
            _pythonTypes.Add(GetTypePtr("PyRange_Type"), PyType.RangeType);
            _pythonTypes.Add(GetTypePtr("PyReversed_Type"), PyType.ReversedType);
            _pythonTypes.Add(GetTypePtr("PySTEntry_Type"), PyType.STEntryType);
            _pythonTypes.Add(GetTypePtr("PySeqIter_Type"), PyType.SeqIterType);
            _pythonTypes.Add(GetTypePtr("PySet_Type"), PyType.SetType);
            _pythonTypes.Add(GetTypePtr("PySlice_Type"), PyType.SliceType);
            _pythonTypes.Add(GetTypePtr("PyStaticMethod_Type"), PyType.StaticMethodType);
            _pythonTypes.Add(GetTypePtr("PyString_Type"), PyType.StringType);
            _pythonTypes.Add(GetTypePtr("PySuper_Type"), PyType.SuperType);
            _pythonTypes.Add(GetTypePtr("PyTasklet_TypePtr"), PyType.TaskletTypePtr);
            _pythonTypes.Add(GetTypePtr("PyTraceBack_Type"), PyType.TraceBackType);
            _pythonTypes.Add(GetTypePtr("PyTuple_Type"), PyType.TupleType);
            _pythonTypes.Add(GetTypePtr("PyType_Type"), PyType.TypeType);
            _pythonTypes.Add(GetTypePtr("PyUnicode_Type"), PyType.UnicodeType);
            _pythonTypes.Add(GetTypePtr("PyWrapperDescr_Type"), PyType.WrapperDescrType);
        }

        public static IntPtr GetTypePtr(string typeName)
        {
            IntPtr procAddress;
            IntPtr pyPtr = IntPtr.Zero;
            if (!_loadedTypes.TryGetValue(typeName, out procAddress))
            {
                if (pyPtr == IntPtr.Zero)
                {
                    pyPtr = PyCall.LoadLibrary("python27.dll");
                }
                procAddress = PyCall.GetProcAddress(pyPtr, typeName);
                if (procAddress != IntPtr.Zero)
                {
                    _loadedTypes[typeName] = procAddress;
                }
            }
            return procAddress;
        }

        public static PyType? GetEveObjectType(IntPtr intptr, bool isDerived = false)
        {
            PyType? returnType;
            if (!_pythontypesLoaded)
            {
                PopulatePythonTypes();
            }
            if (intptr == IntPtr.Zero)
            {
                return PyType.Invalid;
            }
            IntPtr key = Marshal.ReadIntPtr(((IntPtr)intptr.ToInt64() + 4)); // Points to the type
            if (!_pythonTypes.TryGetValue(key, out returnType) && !isDerived)
            {
                returnType = GetEveObjectType(key, true);
                _pythonTypes.Add(key, returnType);
                return returnType;
            }
            if (isDerived)
            {
                string str = returnType.ToString();
                returnType = (PyType)Enum.Parse(typeof(PyType), "Derived" + str);
            }
            return returnType;
        }
        #endregion
    }
}
