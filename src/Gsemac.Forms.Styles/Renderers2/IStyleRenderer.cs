namespace Gsemac.Forms.Styles.Renderers2 {

    public interface IStyleRenderer {

        void Render(object obj, IRenderContext context);

    }

    public interface IStyleRenderer<T> :
        IStyleRenderer {

        void Render(T obj, IRenderContext context);

    }

}