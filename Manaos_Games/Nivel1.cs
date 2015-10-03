using TgcViewer;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
using System.Collections.Generic;
using Microsoft.DirectX;

namespace AlumnoEjemplos.Manaos_Games
{
    public class Nivel1
    {
        private TgcScene escenario;
        private TgcSkyBox skyBox;
        private Cuadro cuadroPared;
        private List<TgcBoundingBox> obstaculos;
        private List<TgcMesh> seleccionables;

        TgcBox escalon;
        TgcBox escalon2;

        public List<TgcBoundingBox> Obstaculos
        {
            get { return obstaculos; }
        }

        public List<TgcMesh> Seleccionables
        {
            get { return seleccionables; }
        }

        public Nivel1()
        {
            //Carpeta de archivos Media del alumno
            string alumnoMediaFolder = GuiController.Instance.AlumnoEjemplosMediaDir;

            //Device de DirectX para crear primitivas
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            TgcSceneLoader loader = new TgcSceneLoader();

            obstaculos = new List<TgcBoundingBox>();
            seleccionables = new List<TgcMesh>();


            skyBox = new TgcSkyBox();
            skyBox.Size = new Vector3(8000, 2000, 8000);
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
         
            foreach (TgcMesh pared in escenario.Meshes)
            {
                obstaculos.Add(pared.BoundingBox);
            }

            escalon = TgcBox.fromSize(new Vector3(escenario.BoundingBox.PMax.X - 500f,
                                                  escenario.BoundingBox.PMin.Y + 150f,
                                                  escenario.BoundingBox.calculateBoxCenter().Z),
                                             new Vector3(1000f, 300f, escenario.BoundingBox.calculateAxisRadius().Z * 2),
                                             TgcTexture.createTexture(alumnoMediaFolder + "Textures\\" + "lana.jpg"));

            escalon.render();

            obstaculos.Add(escalon.BoundingBox);

            escalon2 = TgcBox.fromSize(new Vector3(escenario.BoundingBox.PMax.X - 250f,
                                      escenario.BoundingBox.PMin.Y + 450f,
                                      escenario.BoundingBox.calculateBoxCenter().Z),
                                 new Vector3(500f, 300f, escenario.BoundingBox.calculateAxisRadius().Z * 2),
                                 TgcTexture.createTexture(alumnoMediaFolder + "Textures\\" + "paredRugosa.jpg"));

            escalon2.render();

            obstaculos.Add(escalon2.BoundingBox);

            seleccionables.Add(cuadroPared.GetMesh());
        }

        public void Render()
        {
            escenario.renderAll();
            skyBox.render();
            cuadroPared.Render();
            escalon.render();
            escalon2.render();
        }

        public Vector3 posicionInicial()
        {
            return new Vector3(escenario.BoundingBox.PMin.X + 200f,
                               escenario.BoundingBox.PMin.Y + 200f,
                               escenario.BoundingBox.calculateBoxCenter().Z);
        }

        public Vector3 orientacionCamara()
        {
            return new Vector3(escenario.BoundingBox.PMax.X,
                               0,
                               escenario.BoundingBox.calculateBoxCenter().Z);
        }
    }
}
