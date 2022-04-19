using System;

namespace Gsemac.Forms.Styles.Renderers2 {

    public interface IStyleRendererFactory {

        IStyleRenderer Create(Type forType);

    }

}