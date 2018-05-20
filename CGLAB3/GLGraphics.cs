using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CGLAB3
{
    class GLGraphics
    {
        Vector3 cameraPosition = new Vector3(2, 1, 3);
        Vector3 cameraDirecton = new Vector3(0, 0, 0);
        Vector3 cameraUp = new Vector3(0, 0, 1);

        string glVersion;
        string glslVersion;

        int BasicProgramID;
        int BasicVertexShaders;
        int BasicFragmentShader;

        int vaoHandle;

        public void Resize(int width, int height)
        {
            GL.ClearColor(Color.DarkGray);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.DepthTest);


            Matrix4 perspectiveMat = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                width / (float)height,
                1,
                64);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMat);
        }

        public void Update()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 viewMat = Matrix4.LookAt(cameraPosition, cameraDirecton, cameraUp);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMat);
            Render();
        }


        private void drawTestQuad()
        {
            //GL.Begin(PrimitiveType.Quads);
            //GL.Color3(Color.Blue);
            //GL.Vertex3(-1.0f, -1.0f, -1.0f);
            //GL.Color3(Color.Red);
            //GL.Vertex3(-1.0f, 1.0f, -1.0f);
            //GL.Color3(Color.White);
            //GL.Vertex3(1.0f, 1.0f, -1.0f);
            //GL.Color3(Color.Green);
            //GL.Vertex3(1.0f, -1.0f, -1.0f);
            //GL.End();
            InitShaders();
            GL.UseProgram(BasicProgramID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            
        }

        public void Render()
        {
            drawTestQuad();
        }

        public void loadShader(String filename, ShaderType type, int program, out int address)
        {
            glVersion = GL.GetString(StringName.Version);
            glslVersion = GL.GetString(StringName.ShadingLanguageVersion);

            address = GL.CreateShader(type);

            if (address == 0)
            {
                throw new Exception("Error, can't create shader");
            }
            using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
                Console.WriteLine(GL.GetShaderInfoLog(address));
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
            //{
            //    GL.ShaderSource(address, sr.ReadToEnd());
            //}
            //GL.CompileShader(address);
            //GL.AttachShader(program, address);
            //Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        private void InitShaders()
        {
            BasicProgramID = GL.CreateProgram();
            loadShader("D:\\Vstudio\\Git\\CGLab3\\CGLAB3\\basic.vs.txt", ShaderType.VertexShader, BasicProgramID,
                out BasicVertexShaders);
            loadShader("D:\\Vstudio\\Git\\CGLab3\\CGLAB3\\basic.fs.txt", ShaderType.FragmentShader, BasicProgramID,
                out BasicFragmentShader);

            GL.LinkProgram(BasicProgramID);

            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);

            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));

            float[] positionData = { -0.8f, -0.8f, 0.0f, 0.8f, -0.8f, 0.0f, 0.0f, 0.8f, 0.0f };
            float[] colorData = { 1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f };

            int[] vboHandlers = new int[2];
            GL.GenBuffers(2, vboHandlers);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);

            GL.BufferData(BufferTarget.ArrayBuffer,
                (IntPtr)(sizeof(float) * positionData.Length),
                positionData, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);
            GL.BufferData(BufferTarget.ArrayBuffer,
                (IntPtr)(sizeof(float) * colorData.Length),
                colorData, BufferUsageHint.StaticDraw);

            vaoHandle = GL.GenVertexArray();
            GL.BindVertexArray(vaoHandle);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        public void CloseProgram()
        {
            GL.UseProgram(0);
        }

    }
}
