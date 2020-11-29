using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public interface IListViewRenderer {

        void DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e);
        void DrawItem(object sender, DrawListViewItemEventArgs e);
        void DrawSubItem(object sender, DrawListViewSubItemEventArgs e);

    }

}