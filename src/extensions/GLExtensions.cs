
using System.Runtime.CompilerServices;
using OpenTK.Graphics.OpenGL4;

namespace FrogLib;

public static class GLExtensions {
    extension(GL) {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateBuffer() {
            GL.CreateBuffers(1, out int id);
            return id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateVertexArray() {
            GL.CreateVertexArrays(1, out int id);
            return id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateTexture(TextureTarget target) {
            GL.CreateTextures(target, 1, out int id);
            return id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateSampler() {
            GL.CreateSamplers(1, out int id);
            return id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateFramebuffer() {
            GL.CreateFramebuffers(1, out int id);
            return id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateRenderbuffer() {
            GL.CreateRenderbuffers(1, out int id);
            return id;
        }
    }
}