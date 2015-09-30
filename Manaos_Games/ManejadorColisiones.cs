using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer;
using Microsoft.DirectX.Direct3D;

namespace AlumnoEjemplos.Manaos_Games
{
    class ManejadorColisiones
    {
        private Vector3 antCamPos;
        private float time;
        private bool firstTime;

        //Variables de colision
        private float traceRatio;
        private int traceType;
        private float traceRadius;
        private Vector3 vCollisionNormal;

        // Almacena si colisiono o no
        private bool bCollided;
        private bool bTryStep;
        private Vector3 velocidad;

        //Boundingbox de la camara
        private Vector3 vTraceMaxs;
        private Vector3 vTraceMins;
        private Vector3 vExtents;

        private Camara fpsCamara;

        private List<TgcBoundingBox> obstaculos;

        public Camara FPSCamara
        {
            get { return fpsCamara; }
        }

        private bool enTierra;
   
        private bool EnTierra
        {
            get { return enTierra; }
        }

        private float velSalto;

        public float VelocidadSalto
        {
            get { return velSalto; }
            set { velSalto = value; }
        }

        private float gravedad;

        public float Gravedad
        {
            get { return gravedad; }
            set { gravedad = value; }
        }

        private TgcBoundingBox jugadorPriPers;

        public TgcBoundingBox Jugador
        {
            get { return jugadorPriPers; }
            set { jugadorPriPers = value; }
        }

        public ManejadorColisiones(Camara camara, List<TgcBoundingBox> obstEscenario)
        {
            this.fpsCamara = camara;

            velSalto = 80.0f;
            gravedad = 80.0f;

            velocidad = new Vector3();
            antCamPos = Vector3.Empty;
            time = 0;
            firstTime = true;
            jugadorPriPers = new TgcBoundingBox(new Vector3(-20, -60, -20), new Vector3(20, 20, 20));

            obstaculos = obstEscenario;
        }

        public Vector3 update()
        {
            Device device = GuiController.Instance.D3dDevice;
            float elapsedTime = GuiController.Instance.ElapsedTime;

            fpsCamara.updateCamera();

            if (GuiController.Instance.D3dInput.keyPressed(Microsoft.DirectX.DirectInput.Key.Space))
            {
                //Salta si esta en el piso
                if (enTierra)
                {
                    //Vector3 velocity = GuiController.Instance.FpsCamera.Velocity;
                    velocidad.Y = velSalto;
                }
            }

            time += elapsedTime;
            Vector3 camPos = fpsCamara.getPosition();
            Vector3 camLookAt = fpsCamara.getLookAt();

            if (!firstTime)
            {
                Vector3 lookDir = camLookAt - camPos;

               // Vector3 aceleracion = new Vector3(0, -gravedad, 0);

                //aplico la gravedad
                //velocidad = velocidad + elapsedTime * aceleracion;

                //camPos = camPos + velocidad * elapsedTime;
                //camPos.Y -= kEpsilon * 1.5f;

                TgcBoundingBox camaraBoundingBox = new TgcBoundingBox(antCamPos, camPos);

                bool collide = false;

                if (antCamPos != camPos)
                {
                    foreach (TgcBoundingBox obstaculo in obstaculos)
                    {
                        TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(camaraBoundingBox, obstaculo);
                        if (result == TgcCollisionUtils.BoxBoxResult.Adentro || result == TgcCollisionUtils.BoxBoxResult.Atravesando)
                        {
                            collide = true;
                            break;
                        }
                    }

                    //Si hubo colision, restaurar la posicion anterior
                    if (collide)
                    {
                        //Vector3 retroceso = antCamPos - fpsCamara.getPosition();
                        fpsCamara.setCamera(antCamPos, camLookAt);
                        
                    }
                    /*else 
                    {
                        fpsCamara.move(camPos - fpsCamara.getPosition());
                    }*/
                }

                if (enTierra)
                {
                    if (velocidad.Y < 0)
                        velocidad.Y = 0;
                }

            }

            antCamPos = camPos;
            firstTime = false;

            return camPos;
        }

        public Vector3 getCurrentPosition()
        {
            return fpsCamara.getPosition();
        }
     }
 }

