﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Geometry3D.cs" company="Helix Toolkit">
//   Copyright (c) 2014 Helix Toolkit contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



namespace HelixToolkit.Wpf.SharpDX
{
    using System;

    using global::SharpDX;

    using HelixToolkit.Wpf.SharpDX.Core;
    using HelixToolkit.SharpDX.Shared.Utilities;
    using System.Runtime.InteropServices;
    using System.ComponentModel;
    using HelixToolkit.SharpDX.Shared.Model;
    using System.Diagnostics;

#if !NETFX_CORE
    [Serializable]
#endif
    public abstract class Geometry3D : ObservableObject
    {
        public const string VertexBuffer = "VertexBuffer";
        public const string TriangleBuffer = "TriangleBuffer";

        private IntCollection indices = null;
        public IntCollection Indices
        {
            get
            {
                return indices;
            }
            set
            {
                if(Set<IntCollection>(ref indices, value))
                {
                    Octree = null;
                }
            }
        }

        private Vector3Collection position = null;
        public Vector3Collection Positions
        {
            get
            {
                return position;
            }
            set
            {
                if(Set<Vector3Collection>(ref position, value))
                {
                    Octree = null;
                }
            }
        }

        private Color4Collection colors = null;
        public Color4Collection Colors
        {
            get
            {
                return colors;
            }
            set
            {
                Set<Color4Collection>(ref colors, value);
            }
        }

        public struct Triangle
        {
            public Vector3 P0, P1, P2;
        }

        public struct Line
        {
            public Vector3 P0, P1;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct PointsVertex
        {
            public Vector4 Position;
            public Color4 Color;
            public const int SizeInBytes = 4 * (4 + 4);
        }

        public struct Point
        {
            public Vector3 P0;
        }

        /// <summary>
        /// TO use Octree during hit test to improve hit performance, please call UpdateOctree after model created.
        /// </summary>
        public IOctree Octree { private set; get; }

        /// <summary>
        /// Call to manually update vertex buffer. Use with <see cref="DisablePropertyChangedEvent"/>
        /// </summary>
        public void UpdateVertices()
        {
            RaisePropertyChanged(VertexBuffer);
        }
        /// <summary>
        /// Call to manually update triangle buffer. Use with <see cref="DisablePropertyChangedEvent"/>
        /// </summary>
        public void UpdateTriangles()
        {
            RaisePropertyChanged(TriangleBuffer);
        }

        /// <summary>
        /// Create Octree for current model.
        /// </summary>
        public void UpdateOctree()
        {
            if (Positions != null && Indices != null && Positions.Count > 0 && Indices.Count > 0)
            {
                this.Octree = CreateOctree();
                this.Octree?.BuildTree();
            }
            else
            {
                this.Octree = null;
            }
        }
        /// <summary>
        /// Override to create different octree in subclasses.
        /// </summary>
        /// <returns></returns>
        protected virtual IOctree CreateOctree()
        {
            return null;
        }

        /// <summary>
        /// Set octree to null
        /// </summary>
        public void ClearOctree()
        {
            Octree = null;
        }
    }
}
