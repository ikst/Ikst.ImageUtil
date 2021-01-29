using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ikst.ImageUtil
{
    /// <summary>
    /// 画像変換モード
    /// </summary>
    public enum ResizeMode
    {
        /// <summary>元画像を伸縮しない。拡大した場合は余白が出来る。</summary>
        None = 1,

        /// <summary>縦横比を維持しないで元画像を伸縮する。余白は出来ない。</summary>
        Fill,

        /// <summary>縦横比を維持して元画像を伸縮する。元画像と縦横比が異なる場合、余白ができる。</summary>
        Uniform,

        /// <summary>縦横比を維持して元画像を伸縮する。元画像と縦横比が異なる場合であっても、余白はできない。元画像の一部が欠落する</summary>
        UniformToFill,

        /// <summary>縦横比を維持して変換後サイズをリサイズする。余白はできない。（元画像サイズと変換後サイズの縦横比が一致した状態でのFill）</summary>
        FixedAspectRatioResize
    }
}
