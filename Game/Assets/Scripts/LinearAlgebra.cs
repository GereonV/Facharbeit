using System;

namespace LinearAlgebra {

    [Serializable]
    public class Vector {

        public int Size {
            get => rows.Length;
        }

        private float[] rows;
        public float this[in int i] {
            get => i < 0 || i >= rows.Length ? float.NaN : rows[i];
        }

        public Vector(in float[] values) {
            if(values.Length == 0)
                throw new ArgumentException("Empty Vector is not allowed", nameof(values));
            rows = new float[values.Length];
            for(int i = 0; i < values.Length; i++)
                rows[i] = values[i];
        }

        public static Vector operator +(in Vector vector1, in Vector vector2) {
            if(vector1.Size != vector2.Size)
                throw new ArgumentException("Only same-sized Vectors can be added", $"{nameof(vector1)} & {nameof(vector2)}");
            float[] values = new float[vector1.Size];
            for(int i = 0; i < vector1.Size; i++)
                values[i] = vector1[i] + vector2[i];
            return new Vector(values);
        }

        public static Vector operator *(in float multiplier, in Vector vector) {
            float[] values = new float[vector.Size];
            for(int i = 0; i < vector.Size; i++)
                values[i] = multiplier * vector[i];
            return new Vector(values);
        }

        public override string ToString() {
            string rowRepresentation = $"{rows[0]}";
            for(int i = 1; i < Size; i++)
                rowRepresentation += $", {rows[i]}";
            return $"Vector ({Size}): [{rowRepresentation}]";
        }

    }

    [Serializable]
    public class Matrix {

        public int[] Size {
            get => new int[] {columns[0].Size, columns.Length};
        }

        private Vector[] columns;
        private Vector this[in int i] {
            get => i < 0 || i >= columns.Length ? null : columns[i];
        }
        public float this[in int i, in int j] {
            get => i < 0 || i >= columns.Length ? float.NaN : columns[i][j];
        }

        public Matrix(in float[][] values) {
            if(values.Length == 0)
                throw new ArgumentException("Empty Matrix is not allowed", nameof(values));
            int size = values[0].Length;
            columns = new Vector[values.Length];
            for(int i = 0; i < values.Length; i++) {
                if(values[i].Length != size)
                    throw new ArgumentException("Matrix has to be rectangluar", nameof(values));
                columns[i] = new Vector(values[i]);
            }
        }

        public static Vector operator *(in Matrix matrix, in Vector vector) {
            if(matrix.Size[1] != vector.Size)
                throw new ArgumentException("Vector's Size doesn't match Matrix's Width", $"{nameof(vector)} & {nameof(matrix)}");
            Vector result = new Vector(new float[matrix.Size[0]]);
            for(int i = 0; i < vector.Size; i++)
                result += vector[i] * matrix[i];
            return result;
        }

        public override string ToString() {
            string columnRepresentation = columns[0].ToString();
            for(int i = 1; i < Size[1]; i++)
                columnRepresentation += $", {columns[i].ToString()}";
            return $"Matrix ({Size[0]}, {Size[1]}): [{columnRepresentation}]";
        }

    }

}