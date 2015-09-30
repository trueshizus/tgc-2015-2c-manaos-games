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
        //Floor piso;
        private TgcScene escenario;
        private TgcSkyBox skyBox;
        private Cuadro cuadroPared;
        List<TgcBoundingBox> obstaculos;
        List<TgcMesh> seleccionables;

        ManejadorColisiones colisionador;

        TgcPickingRay pickingRay;
        Vector3 collisionPoint;
        //TgcBox collisionPointMesh;
        bool selected;
        TgcMesh selectedMesh;

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

            Vector3 posicionCamara;

            //Device de DirectX para crear primitivas
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            TgcSceneLoader loader = new TgcSceneLoader();

            obstaculos = new List<TgcBoundingBox>();
            seleccionables = new List<TgcMesh>();

            //Carpeta de archivos Media del alumno
            string alumnoMediaFolder = GuiController.Instance.AlumnoEjemplosMediaDir;

            skyBox = new TgcSkyBox();
            skyBox.Size = new Vector3(8000, 2000, 8000);

            //Configurar color
            //skyBox.Color = Color.OrangeRed;

            //Configurar las texturas para cada una de las 6 caras
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, alumnoMediaFolder + "Textures\\" + "city_top.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, alumnoMediaFolder + "Textures\\" + "city_down.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, alumnoMediaFolder + "Textures\\" + "city_left.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, alumnoMediaFolder + "Textures\\" + "city_right.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, alumnoMediaFolder + "Textures\\" + "city_front.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, alumnoMediaFolder + "Textures\\" + "city_back.jpg");
           
            escenario = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + "\\Nivel1-TgcScene.xml");

            skyBox.Center = escenario.BoundingBox.calculateBoxCenter();
         
            //Actualizar todos los valores para crear el SkyBox
            skyBox.updateValues();

            cuadroPared = new Cuadro(new Vector3(escenario.BoundingBox.calculateBoxCenter().X,
                                                 escenario.BoundingBox.PMin.Y + 300f, 
                                                 escenario.BoundingBox.PMin.Z),
                                       new Vector3(500, 250, 25));
         
            fpsCamara = new Camara();

            GuiController.Instance.CurrentCamera = fpsCamara;

            fpsCamara.MovementSpeed = 1000f;
            fpsCamara.RotationSpeed = 2f;
            
            posicionCamara = new Vector3(escenario.BoundingBox.calculateBoxCenter().X,
                                            escenario.BoundingBox.PMin.Y + 200f,
                                           escenario.BoundingBox.calculateBoxCenter().Z);

            fpsCamara.setCamera(posicionCamara, posicionCamara + new Vector3(1.0f, 0.0f, 0.0f));
            
            fpsCamara.updateCamera();

       
            foreach (TgcMesh pared in escenario.Meshes)
            {
                obstaculos.Add(pared.BoundingBox);
            }

            colisionador = new ManejadorColisiones(fpsCamara, obstaculos);
            
            
            seleccionables.Add(cuadroPared.GetMesh());

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
                foreach (TgcMesh objeto in seleccionables)
                {
                    TgcBoundingBox aabb = objeto.BoundingBox;

                    //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                    selected = TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, aabb, out collisionPoint);

                    if (selected)
                    {
                        selectedMesh = objeto;
                        break;
                    }
                }
            }

            escenario.renderAll();
            skyBox.render();
            cuadroPared.Render();
            //fpsCamara.updateCamera();

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
