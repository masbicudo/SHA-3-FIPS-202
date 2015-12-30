using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHA3
{
    public struct StateArray1600
    {
        private readonly ulong A00;
        private readonly ulong A01;
        private readonly ulong A02;
        private readonly ulong A03;
        private readonly ulong A04;
        private readonly ulong A10;
        private readonly ulong A11;
        private readonly ulong A12;
        private readonly ulong A13;
        private readonly ulong A14;
        private readonly ulong A20;
        private readonly ulong A21;
        private readonly ulong A22;
        private readonly ulong A23;
        private readonly ulong A24;
        private readonly ulong A30;
        private readonly ulong A31;
        private readonly ulong A32;
        private readonly ulong A33;
        private readonly ulong A34;
        private readonly ulong A40;
        private readonly ulong A41;
        private readonly ulong A42;
        private readonly ulong A43;
        private readonly ulong A44;

        public StateArray1600(ulong a00, ulong a01, ulong a02, ulong a03, ulong a04, ulong a10, ulong a11, ulong a12, ulong a13, ulong a14, ulong a20, ulong a21, ulong a22, ulong a23, ulong a24, ulong a30, ulong a31, ulong a32, ulong a33, ulong a34, ulong a40, ulong a41, ulong a42, ulong a43, ulong a44) : this()
        {
            this.A00 = a00;
            this.A01 = a01;
            this.A02 = a02;
            this.A03 = a03;
            this.A04 = a04;
            this.A10 = a10;
            this.A11 = a11;
            this.A12 = a12;
            this.A13 = a13;
            this.A14 = a14;
            this.A20 = a20;
            this.A21 = a21;
            this.A22 = a22;
            this.A23 = a23;
            this.A24 = a24;
            this.A30 = a30;
            this.A31 = a31;
            this.A32 = a32;
            this.A33 = a33;
            this.A34 = a34;
            this.A40 = a40;
            this.A41 = a41;
            this.A42 = a42;
            this.A43 = a43;
            this.A44 = a44;
        }

        public static StateArray1600 θ(StateArray1600 A)
        {
            ulong C0 = A.A00 ^ A.A01 ^ A.A02 ^ A.A03 ^ A.A04;
            ulong C1 = A.A10 ^ A.A11 ^ A.A12 ^ A.A13 ^ A.A14;
            ulong C2 = A.A20 ^ A.A21 ^ A.A22 ^ A.A23 ^ A.A24;
            ulong C3 = A.A30 ^ A.A31 ^ A.A32 ^ A.A33 ^ A.A34;
            ulong C4 = A.A40 ^ A.A41 ^ A.A42 ^ A.A43 ^ A.A44;

            ulong D0 = A.A40 ^ C1.RotateLeft(1);
            ulong D1 = A.A00 ^ C2.RotateLeft(1);
            ulong D2 = A.A10 ^ C3.RotateLeft(1);
            ulong D3 = A.A20 ^ C4.RotateLeft(1);
            ulong D4 = A.A30 ^ C0.RotateLeft(1);

            ulong A00 = A.A00 ^ D0;
            ulong A01 = A.A01 ^ D0;
            ulong A02 = A.A02 ^ D0;
            ulong A03 = A.A03 ^ D0;
            ulong A04 = A.A04 ^ D0;
            ulong A10 = A.A10 ^ D1;
            ulong A11 = A.A11 ^ D1;
            ulong A12 = A.A12 ^ D1;
            ulong A13 = A.A13 ^ D1;
            ulong A14 = A.A14 ^ D1;
            ulong A20 = A.A20 ^ D2;
            ulong A21 = A.A21 ^ D2;
            ulong A22 = A.A22 ^ D2;
            ulong A23 = A.A23 ^ D2;
            ulong A24 = A.A24 ^ D2;
            ulong A30 = A.A30 ^ D3;
            ulong A31 = A.A31 ^ D3;
            ulong A32 = A.A32 ^ D3;
            ulong A33 = A.A33 ^ D3;
            ulong A34 = A.A34 ^ D3;
            ulong A40 = A.A40 ^ D4;
            ulong A41 = A.A41 ^ D4;
            ulong A42 = A.A42 ^ D4;
            ulong A43 = A.A43 ^ D4;
            ulong A44 = A.A44 ^ D4;

            return new StateArray1600(
                A00,
                A01,
                A02,
                A03,
                A04,
                A10,
                A11,
                A12,
                A13,
                A14,
                A20,
                A21,
                A22,
                A23,
                A24,
                A30,
                A31,
                A32,
                A33,
                A34,
                A40,
                A41,
                A42,
                A43,
                A44);
        }

        public static StateArray1600 ρ(StateArray1600 A)
        {
            ulong C0 = A.A00 ^ A.A01 ^ A.A02 ^ A.A03 ^ A.A04;
            ulong C1 = A.A10 ^ A.A11 ^ A.A12 ^ A.A13 ^ A.A14;
            ulong C2 = A.A20 ^ A.A21 ^ A.A22 ^ A.A23 ^ A.A24;
            ulong C3 = A.A30 ^ A.A31 ^ A.A32 ^ A.A33 ^ A.A34;
            ulong C4 = A.A40 ^ A.A41 ^ A.A42 ^ A.A43 ^ A.A44;

            ulong D0 = A.A40 ^ C1.RotateLeft(1);
            ulong D1 = A.A00 ^ C2.RotateLeft(1);
            ulong D2 = A.A10 ^ C3.RotateLeft(1);
            ulong D3 = A.A20 ^ C4.RotateLeft(1);
            ulong D4 = A.A30 ^ C0.RotateLeft(1);

            ulong A00 = A.A00 ^ D0;
            ulong A01 = A.A01 ^ D0;
            ulong A02 = A.A02 ^ D0;
            ulong A03 = A.A03 ^ D0;
            ulong A04 = A.A04 ^ D0;
            ulong A10 = A.A10 ^ D1;
            ulong A11 = A.A11 ^ D1;
            ulong A12 = A.A12 ^ D1;
            ulong A13 = A.A13 ^ D1;
            ulong A14 = A.A14 ^ D1;
            ulong A20 = A.A20 ^ D2;
            ulong A21 = A.A21 ^ D2;
            ulong A22 = A.A22 ^ D2;
            ulong A23 = A.A23 ^ D2;
            ulong A24 = A.A24 ^ D2;
            ulong A30 = A.A30 ^ D3;
            ulong A31 = A.A31 ^ D3;
            ulong A32 = A.A32 ^ D3;
            ulong A33 = A.A33 ^ D3;
            ulong A34 = A.A34 ^ D3;
            ulong A40 = A.A40 ^ D4;
            ulong A41 = A.A41 ^ D4;
            ulong A42 = A.A42 ^ D4;
            ulong A43 = A.A43 ^ D4;
            ulong A44 = A.A44 ^ D4;

            return new StateArray1600(
                A00,
                A01,
                A02,
                A03,
                A04,
                A10,
                A11,
                A12,
                A13,
                A14,
                A20,
                A21,
                A22,
                A23,
                A24,
                A30,
                A31,
                A32,
                A33,
                A34,
                A40,
                A41,
                A42,
                A43,
                A44);
        }
    }

    public static class IntegerExtensions
    {
        public static byte RotateLeft(this byte value, int count)
        {
            return (byte)(((uint)value << count) | ((uint)value >> (8 - count)));
        }

        public static byte RotateRight(this byte value, int count)
        {
            return (byte)(((uint)value >> count) | ((uint)value << (8 - count)));
        }

        public static ushort RotateLeft(this ushort value, int count)
        {
            return (ushort)(((uint)value << count) | ((uint)value >> (16 - count)));
        }

        public static ushort RotateRight(this ushort value, int count)
        {
            return (ushort)(((uint)value >> count) | ((uint)value << (16 - count)));
        }

        public static uint RotateLeft(this uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        public static uint RotateRight(this uint value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }

        public static ulong RotateLeft(this ulong value, int count)
        {
            return (value << count) | (value >> (64 - count));
        }

        public static ulong RotateRight(this ulong value, int count)
        {
            return (value >> count) | (value << (64 - count));
        }
    }
}
