using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using TgcViewer.Utils.Input;
using TgcViewer;
using Microsoft.DirectX.Direct3D;

namespace AlumnoEjemplos.Manaos_Games
{
    class Camara : TgcCamera
    {
        Vector3 eye = new Vector3();
        Vector3 target = new Vector3();
        Vector3 up = new Vector3(0.0f, 1.0f, 0.0f);
        private Matrix viewMatrix = Matrix.Identity;
        private Vector3 forwardDirection = new Vector3();
        private Vector3 sideDirection = new Vector3();
        private bool lockCam = true;
        protected Point mouseCenter;
        private float movementSpeed;
        private float rotationSpeed;
        private float jumpSpeed;
        private float latitud;
        private float longitud;


        public Camara()
        {
            Control focusWindows = GuiController.Instance.D3dDevice.CreationParameters.FocusWindow;
            mouseCenter = focusWindows.PointToScreen(
                new Point(
                    focusWindows.Width / 2,
                    focusWindows.Height / 2)
                    );
        }

        ~Camara()
        {
            LockCam = false;
        }

        //No se usa, solo esta por la herencia
        public bool Enable
        {
            get;
            set;
        }


        public Vector3 ForwardDirection
        {
            get{return forwardDirection;}
        }

        public Vector3 SideDirection
        {
            get{return sideDirection;}
        }

        public bool LockCam
        {
            get { return lockCam; }
            set
            {
                if (!lockCam && value)
                {
                    Cursor.Position = mouseCenter;

                    Cursor.Hide();
                }
                if (lockCam && !value)
                    Cursor.Show();
                lockCam = value;
            }
        }

        public float MovementSpeed
        {
            get { return movementSpeed; }
            set { movementSpeed = value; }
        }

        public float RotationSpeed
        {
            get { return rotationSpeed; }
            set { rotationSpeed = value; }
        }

        public float JumpSpeed
        {
            get { return jumpSpeed; }
            set { jumpSpeed = value; }
        }

        private void recalcularDirecciones()
        {
            Vector3 forward = target - eye;
            forward.Y = 0;
            forward.Normalize();

            forwardDirection = forward;
            sideDirection.X = forward.Z;
            sideDirection.Z = -forward.X;
        }

        #region Miembros de Camara

        public Vector3 getPosition()
        {
            return eye;
        }

        public Vector3 getLookAt()
        {
            return target;
        }

        public void updateCamera()
        {
            float elapsedTime = GuiController.Instance.ElapsedTime;
            //Forward
            if (GuiController.Instance.D3dInput.keyDown(Key.W))
            {
                moveForward(MovementSpeed * elapsedTime);
            }

            //Backward
            if (GuiController.Instance.D3dInput.keyDown(Key.S))
            {
                moveForward(-MovementSpeed * elapsedTime);
            }

            //Strafe right
            if (GuiController.Instance.D3dInput.keyDown(Key.D))
            {
                moveSide(MovementSpeed * elapsedTime);
            }

            //Strafe left
            if (GuiController.Instance.D3dInput.keyDown(Key.A))
            {
                moveSide(-MovementSpeed * elapsedTime);
            }

            //Jump
            if (GuiController.Instance.D3dInput.keyDown(Key.Space))
            {
                moveUp(JumpSpeed * elapsedTime);
            }

            if (GuiController.Instance.D3dInput.keyPressed(Key.L))
            {
                LockCam = !LockCam;
            }

            //Solo rotar si se esta aprentando el boton izq del mouse
            if (lockCam || GuiController.Instance.D3dInput.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                rotate(-GuiController.Instance.D3dInput.XposRelative*rotationSpeed,
                       -GuiController.Instance.D3dInput.YposRelative*rotationSpeed);
            }


            if(lockCam)
                Cursor.Position = mouseCenter;
        }

        public void updateViewMatrix(Microsoft.DirectX.Direct3D.Device d3dDevice)
        {
            viewMatrix = Matrix.LookAtLH(eye, target, up);

            //updateViewMatrix(GuiController.Instance.D3dDevice);

            d3dDevice.Transform.View = viewMatrix;
        }

        #endregion


        public void move(Vector3 v)
        {
            eye.Add(v);
            target.Add(v);
        }

        public void moveForward(float movimiento)
        {
            Vector3 v = ForwardDirection * movimiento;
            move(v);
        }

        public void moveSide(float movimiento)
        {
            Vector3 v = SideDirection * movimiento;
            move(v);
        }

        public void moveUp(float movimiento)
        {
            move(up*movimiento);
        }

        public void rotateY(float movimiento)
        {
            rotate(movimiento, 0);
        }

        public void rotateXZ(float movimiento)
        {
            rotate(0, movimiento);
        }

        public void rotate(float lat, float lon)
        {
            latitud += lat;
            if (latitud >= 360)
                latitud -= 360;

            if (latitud < 0)
                latitud += 360;

            
            longitud += lon;
            if (longitud > 90)
                longitud = 90;
            if (longitud < 0)
                longitud = 0;
            
            recalcularTarget();
        }

        private const float LADO_CUBO = 1.0f;
        private const float MEDIO_LADO_CUBO = LADO_CUBO*0.5f;
        private float STEP_ANGULO = LADO_CUBO/90;
        private void recalcularTarget()
        {
            float x = 0;
            float y = 0;
            float z = 0;
            
            if (latitud < 180)
            {
                if(latitud < 90)
                {
                    z = latitud*STEP_ANGULO;
                }
                else
                {
                    z = LADO_CUBO;
                    x = (latitud - 90)*STEP_ANGULO;
                }
                z = z - MEDIO_LADO_CUBO;
                x = MEDIO_LADO_CUBO - x;
            }
            else
            {
                if (latitud < 270)
                {
                    z = (latitud - 180)*STEP_ANGULO;
                }
                else
                {
                    z = LADO_CUBO;
                    x = (latitud - 270)*STEP_ANGULO;
                }
                z = MEDIO_LADO_CUBO - z;
                x = x - MEDIO_LADO_CUBO;
            }

            y = longitud * STEP_ANGULO - MEDIO_LADO_CUBO;
             


            target = eye + new Vector3(x, y, z);

            recalcularDirecciones();
        }


        public void setCamera(Vector3 eye, Vector3 target)
        {
            this.eye = eye;
            this.target = target;

            Vector3 dir = eye - target;

            if (Math.Abs(dir.X) > 0)
                latitud = (180 * (float)Math.Atan(dir.Z / dir.X)) / (float)Math.PI + 45;
            else
                latitud = 135;

            longitud = (180 * (float)Math.Atan(dir.Y / Math.Sqrt(dir.X*dir.X + dir.Z*dir.Z))) / (float)Math.PI + 45;

            
            rotateY(0);


            recalcularDirecciones();
        }
    }
}