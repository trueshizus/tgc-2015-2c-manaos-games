using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer;

namespace AlumnoEjemplos.Manaos_Games
{
    public class Cuadro
    {
        TgcBox box;

        public Cuadro(Vector3 ubicacion, Vector3 tamanio)
        {
            Vector3 center = ubicacion;
            Vector3 size = tamanio;
            TgcTexture texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "Textures\\palermoFranchescoli.jpg");

            box = TgcBox.fromSize(center, size);

            box.setTexture(texture);
        }

        public void Render()
        {
            box.render();
        }
        
    }
}
