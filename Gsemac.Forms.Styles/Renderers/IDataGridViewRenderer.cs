using System.Windows.Forms;

namespace Gsemac.Forms.Styles.Renderers {

    public interface IDataGridViewRenderer {

        void RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e);
        void RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e);
        void CellPainting(object sender, DataGridViewCellPaintingEventArgs e);
        void Paint(object sender, PaintEventArgs e);

    }

}