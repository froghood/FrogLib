using OpenTK.Mathematics;

namespace FrogLib;

public static class BoxExtensions {

    extension(in Box2 box) {
        public bool FullyContains(in Box2 other) {
            return
                other.Min.X >= box.Min.X && other.Min.Y >= box.Min.Y &&
                other.Max.X <= box.Max.X && other.Max.Y <= box.Max.Y;
        }
    }



    extension(in Box2i box) {
        public bool FullyContains(in Box2i other) {
            return
                other.Min.X >= box.Min.X && other.Min.Y >= box.Min.Y &&
                other.Max.X <= box.Max.X && other.Max.Y <= box.Max.Y;
        }
    }



    extension(in Box3 box) {
        public bool FullyContains(in Box3 other) {
            return
                other.Min.X >= box.Min.X && other.Min.Y >= box.Min.Y && other.Min.Z >= box.Min.Z &&
                other.Max.X <= box.Max.X && other.Max.Y <= box.Max.Y && other.Max.Z <= box.Max.Z;
        }
    }



    extension(in Box3i box) {
        public bool FullyContains(in Box3i other) {
            return
                other.Min.X >= box.Min.X && other.Min.Y >= box.Min.Y && other.Min.Z >= box.Min.Z &&
                other.Max.X <= box.Max.X && other.Max.Y <= box.Max.Y && other.Max.Z <= box.Max.Z;
        }
    }
}