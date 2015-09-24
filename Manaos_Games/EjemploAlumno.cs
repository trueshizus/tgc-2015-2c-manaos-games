using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.Terrain;

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
            Device d3dDevice = GuiController.Instance.D3dDevice;
            TgcSceneLoader loader = new TgcSceneLoader();

            //Carpeta de archivos Media del alumno
            string alumnoMediaFolder = GuiController.Instance.AlumnoEjemplosMediaDir;

            skyBox = new TgcSkyBox();
            skyBox.Size = new Vector3(8000, 3000, 8000);

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


            ///////////////CONFIGURAR CAMARA ROTACIONAL//////////////////
            //Es la camara que viene por default, asi que no hace falta hacerlo siempre
            //GuiController.Instance.RotCamera.Enable = true;
            //Configurar centro al que se mira y distancia desde la que se mira
            //GuiController.Instance.RotCamera.setCamera(new Vector3(0, 0, 0), 100);

         
            //FPS Camara
            GuiController.Instance.FpsCamera.Enable = true;
            GuiController.Instance.FpsCamera.setCamera(new Vector3(escenario.BoundingBox.calculateBoxCenter().X, 
                                                                   escenario.BoundingBox.PMin.Y + 100f, 
                                                                   escenario.BoundingBox.calculateBoxCenter().Z), 
                                                       new Vector3(-140f, 40f, -120f));
            GuiController.Instance.FpsCamera.MovementSpeed = 200f;
            GuiController.Instance.FpsCamera.JumpSpeed = 200f;
            

            ///////////////LISTAS EN C#//////////////////
            //crear
            List<string> lista = new List<string>();

            //agregar elementos
            lista.Add("elemento1");
            lista.Add("elemento2");

            //obtener elementos
            string elemento1 = lista[0];

            //bucle foreach
            foreach (string elemento in lista)
            {
                //Loggear por consola del Framework
                GuiController.Instance.Logger.log(elemento);
            }

            //bucle for
            for (int i = 0; i < lista.Count; i++)
            {
                string element = lista[i];
            }


        }


        /// <summary>
        /// Método que se llama cada vez que hay que refrescar la pantalla.
        /// Escribir aquí todo el código referido al renderizado.
        /// Borrar todo lo que no haga falta
        /// </summary>
        /// <param name="elapsedTime">Tiempo en segundos transcurridos desde el último frame</param>
        public override void render(float elapsedTime)
        {
            //Device de DirectX para renderizar
            Device d3dDevice = GuiController.Instance.D3dDevice;

            escenario.renderAll();
            skyBox.render();

            ///////////////INPUT//////////////////
            //conviene deshabilitar ambas camaras para que no haya interferencia

            //Capturar Input teclado 
            if (GuiController.Instance.D3dInput.keyPressed(Microsoft.DirectX.DirectInput.Key.F))
            {
                //Tecla F apretada
            }

            //Capturar Input Mouse
            if (GuiController.Instance.D3dInput.buttonPressed(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                //Boton izq apretado
            }

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
