using AlumnoEjemplos.Manaos_Games;
//using Microsoft.DirectX.Direct3D;
//using Microsoft.DirectX.DirectInput;
using TgcViewer;
using TgcViewer.Example;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using System.Collections.Generic;

namespace AlumnoEjemplos.MiGrupo
{
    /// <summary>
    /// Ejemplo del alumno
    /// </summary>
    public class EjemploAlumno : TgcExample
    {
        /// <summary>
        /// Categoría a la que pertenece el ejemplo.
        /// Influye en donde se va a haber en el árbol de la derecha de la pantalla.
        /// </summary>
        /// 
        ManejadorColisiones colisionador;

        TgcPickingRay pickingRay;
        Vector3 selectionPoint;
        bool applyMovement;
        bool selected;
        TgcMesh selectedMesh;
        Nivel1 nivel;

        Camara fpsCamara;

        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        /// <summary>
        /// Completar nombre del grupo en formato Grupo NN
        /// </summary>
        public override string getName()
        {
            return "Manaos Games";
        }

        /// <summary>
        /// Completar con la descripción del TP
        /// </summary>
        public override string getDescription()
        {
            return " - Se trata de resolver puzzles jugando con las perspectivas de los objetos.";
        }

        /// <summary>
        /// Método que se llama una sola vez,  al principio cuando se ejecuta el ejemplo.
        /// Escribir aquí todo el código de inicialización: cargar modelos, texturas, modifiers, uservars, etc.
        /// Borrar todo lo que no haga falta
        /// </summary>
        public override void init()
        {
            //GuiController.Instance: acceso principal a todas las herramientas del Framework

            //Device de DirectX para crear primitivas
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            TgcSceneLoader loader = new TgcSceneLoader();

            nivel = new Nivel1();
  
            fpsCamara = new Camara();

            GuiController.Instance.CurrentCamera = fpsCamara;

            fpsCamara.MovementSpeed = 1000f;
            fpsCamara.RotationSpeed = 2f;
            
            fpsCamara.setCamera(nivel.posicionInicial(), nivel.orientacionCamara());
            fpsCamara.updateViewMatrix(d3dDevice);

            colisionador = new ManejadorColisiones(fpsCamara, nivel.Obstaculos);
 
            //Iniciarlizar PickingRay
            pickingRay = new TgcPickingRay();
        }


        /// <summary>
        /// Método que se llama cada vez que hay que refrescar la pantalla.
        /// Escribir aquí todo el código referido al renderizado.
        /// Borrar todo lo que no haga falta
        /// </summary>
        /// <param name="elapsedTime">Tiempo en segundos transcurridos desde el último frame</param>
        public override void render(float elapsedTime)
        {
            colisionador.update();

            //Si hacen clic con el mouse, ver si hay colision RayAABB
            if (GuiController.Instance.D3dInput.buttonPressed(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                //Actualizar Ray de colisión en base a posición del mouse
                pickingRay.updateRay();


                //Testear Ray contra el AABB de todos los meshes
                foreach (TgcMesh objeto in nivel.Seleccionables)
                {
                    TgcBoundingBox aabb = objeto.BoundingBox;

                    //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                    selected = TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, aabb, out selectionPoint);

                    if (selected)
                    {
                        selectedMesh = objeto;
                        //Fijar nueva posición destino
                        applyMovement = true;

                        //collisionPointMesh.Position = selectionPoint;
                        //directionArrow.PEnd = new Vector3(selectionPoint.X, 30f, selectionPoint.Z);

                        ////Rotar modelo en base a la nueva dirección a la que apunta
                        //Vector3 direction = Vector3.Normalize(newPosition - mesh.Position);
                        //float angle = FastMath.Acos(Vector3.Dot(originalMeshRot, direction));
                        //Vector3 axisRotation = Vector3.Cross(originalMeshRot, direction);
                        //meshRotationMatrix = Matrix.RotationAxis(axisRotation, angle);
                        break;
                    }
                }
            }

            //Interporlar movimiento, si hay que mover
            if (applyMovement)
            {
                //Ver si queda algo de distancia para mover
                //Vector3 posDiff = newPosition - mesh.Position;
  //              float posDiffLength = posDiff.LengthSq();
  //              if (posDiffLength > float.Epsilon)
  //              {
  //                  //Movemos el mesh interpolando por la velocidad
  //                //  float currentVelocity = speed * elapsedTime;
  //                 // posDiff.Normalize();
  //                 // posDiff.Multiply(currentVelocity);

  //                  //Ajustar cuando llegamos al final del recorrido
  //                //  Vector3 newPos = mesh.Position + posDiff;
  //                //  if (posDiff.LengthSq() > posDiffLength)
  //                //  {
  //                //      newPos = newPosition;
  ////                  }

  //                  //Actualizar flecha de movimiento
  //  //                directionArrow.PStart = new Vector3(mesh.Position.X, 30f, mesh.Position.Z);
  //    //              directionArrow.updateValues();

  //                  //Actualizar posicion del mesh
  //                  mesh.Position = newPos;

  //                  //Como desactivamos la transformacion automatica, tenemos que armar nosotros la matriz de transformacion
  //                  mesh.Transform = meshRotationMatrix * Matrix.Translation(mesh.Position);

  //                  //Actualizar camara
  //                  GuiController.Instance.ThirdPersonCamera.Target = mesh.Position;
  //              }
  //              //Se acabo el movimiento
  //              else
  //              {
  //                  applyMovement = false;
  //              }
            }

            nivel.Render();

            if (selected)
                selectedMesh.BoundingBox.render();
        }

        /// <summary>
        /// Método que se llama cuando termina la ejecución del ejemplo.
        /// Hacer dispose() de todos los objetos creados.
        /// </summary>
        public override void close()
        {

        }

    }
}
