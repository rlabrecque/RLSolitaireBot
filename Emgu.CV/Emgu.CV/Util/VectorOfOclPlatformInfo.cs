﻿//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//  Vector of OclPlatformInfo
//
//  This file is automatically generated, do not modify.
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Wrapped class of the C++ standard vector of OclPlatformInfo.
   /// </summary>
   public partial class VectorOfOclPlatformInfo : Emgu.Util.UnmanagedObject, IInputOutputArray
   {
      private readonly bool _needDispose;
   
      static VectorOfOclPlatformInfo()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create an empty standard vector of OclPlatformInfo
      /// </summary>
      public VectorOfOclPlatformInfo()
         : this(VectorOfOclPlatformInfoCreate(), true)
      {
      }
	  
	   internal VectorOfOclPlatformInfo(IntPtr ptr, bool needDispose)
      {
         _ptr = ptr;
         _needDispose = needDispose;
      }

      /// <summary>
      /// Create an standard vector of OclPlatformInfo of the specific size
      /// </summary>
      /// <param name="size">The size of the vector</param>
      public VectorOfOclPlatformInfo(int size)
         : this( VectorOfOclPlatformInfoCreateSize(size), true)
      {
      }
	  
	   /// <summary>
      /// Create an standard vector of OclPlatformInfo with the initial values
      /// </summary>
      /// <param name="values">The initial values</param>
	   public VectorOfOclPlatformInfo(params Ocl.PlatformInfo[] values)
	     : this()
	   {
         Push(values);
	   }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public int Size
      {
         get
         {
            return VectorOfOclPlatformInfoGetSize(_ptr);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         VectorOfOclPlatformInfoClear(_ptr);
      }
	  
	   /// <summary>
      /// Push a value into the standard vector
      /// </summary>
      /// <param name="value">The value to be pushed to the vector</param>
      public void Push(Ocl.PlatformInfo value)
      {
         VectorOfOclPlatformInfoPush(_ptr, value.Ptr);
      }

      /// <summary>
      /// Push multiple values into the standard vector
      /// </summary>
      /// <param name="values">The values to be pushed to the vector</param>
      public void Push(Ocl.PlatformInfo[] values)
      {
         foreach (Ocl.PlatformInfo value in values)
            Push(value);
      }

      /// <summary>
      /// Push multiple values from the other vector into this vector
      /// </summary>
      /// <param name="other">The other vector, from which the values will be pushed to the current vector</param>
      public void Push(VectorOfOclPlatformInfo other)
      {
         VectorOfOclPlatformInfoPushVector(_ptr, other);
      }
      
	   /// <summary>
      /// Get the item in the specific index
      /// </summary>
      /// <param name="index">The index</param>
      /// <returns>The item in the specific index</returns>
      public Ocl.PlatformInfo this[int index]
      {
         get
         {
		    IntPtr itemPtr = IntPtr.Zero;
            VectorOfOclPlatformInfoGetItemPtr(_ptr, index, ref itemPtr);
            return new Ocl.PlatformInfo(itemPtr, false);
         }
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose && _ptr != IntPtr.Zero)
            VectorOfOclPlatformInfoRelease(ref _ptr);
      }

	   /// <summary>
      /// Get the pointer to cv::_InputArray
      /// </summary>
      public InputArray GetInputArray()
      {
        return new InputArray( cvInputArrayFromVectorOfOclPlatformInfo(_ptr), this );
      }
	  
      /// <summary>
      /// Get the pointer to cv::_OutputArray
      /// </summary>
      public OutputArray GetOutputArray()
      {
         return new OutputArray( cvOutputArrayFromVectorOfOclPlatformInfo(_ptr), this );
      }

	   /// <summary>
      /// Get the pointer to cv::_InputOutputArray
      /// </summary>
      public InputOutputArray GetInputOutputArray()
      {
         return new InputOutputArray( cvInputOutputArrayFromVectorOfOclPlatformInfo(_ptr), this );
      }     
      
      /// <summary>
      /// The size of the item in this Vector, counted as size in bytes.
      /// </summary>
      public static int SizeOfItemInBytes
      {
         get { return VectorOfOclPlatformInfoSizeOfItemInBytes(); }
      }

#if false
      /// <summary>
      /// Create the standard vector of OclPlatformInfo 
      /// </summary>
      public VectorOfOclPlatformInfo(MDMatch[][] values)
         : this()
      {
         using (OclPlatformInfo v = new OclPlatformInfo())
         {
            for (int i = 0; i < values.Length; i++)
            {
               v.Push(values[i]);
               Push(v);
               v.Clear();
            }
         }
      }
	  
	   /// <summary>
      /// Convert the standard vector to arrays of int
      /// </summary>
      /// <returns>Arrays of int</returns>
      public MDMatch[][] ToArrayOfArray()
      {
         int size = Size;
         MDMatch[][] res = new MDMatch[size][];
         for (int i = 0; i < size; i++)
         {
            using (OclPlatformInfo v = this[i])
            {
               res[i] = v.ToArray();
            }
         }
         return res;
      }
#endif

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfOclPlatformInfoCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfOclPlatformInfoCreateSize(int size);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfOclPlatformInfoRelease(ref IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfOclPlatformInfoGetSize(IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfOclPlatformInfoPush(IntPtr v, IntPtr value);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfOclPlatformInfoPushVector(IntPtr ptr, IntPtr otherPtr);
      
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfOclPlatformInfoClear(IntPtr v);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfOclPlatformInfoGetItemPtr(IntPtr vec, int index, ref IntPtr element);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfOclPlatformInfoSizeOfItemInBytes();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvInputArrayFromVectorOfOclPlatformInfo(IntPtr vec);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvOutputArrayFromVectorOfOclPlatformInfo(IntPtr vec);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvInputOutputArrayFromVectorOfOclPlatformInfo(IntPtr vec);
   }
}
