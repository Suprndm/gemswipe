using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GemSwipe.Tests
{
    public class ZIndexTests
    {
        [Test]
        public void ShouldComputeBasicZIndex()
        {
            var skiaParent = new SkiaElement();

            Assert.AreEqual(1, skiaParent.ZIndex);
        }

        [Test]
        public void ShouldComputeZIndexOfChildren()
        {
            var skiaParent = new SkiaElement();

            var skiaChild1 = new SkiaElement();
            var skiaChild2 = new SkiaElement();
            var skiaChild3 = new SkiaElement();

            skiaParent.AddChild(skiaChild1);
            skiaParent.AddChild(skiaChild2);
            skiaParent.AddChild(skiaChild3);

            Assert.Greater(skiaChild3.ZIndex, skiaChild2.ZIndex);
            Assert.Greater(skiaChild2.ZIndex, skiaChild1.ZIndex);

            Assert.Greater(skiaChild1.ZIndex, skiaParent.ZIndex);
            Assert.Greater(skiaChild2.ZIndex, skiaParent.ZIndex);
            Assert.Greater(skiaChild3.ZIndex, skiaParent.ZIndex);
        }

        [Test]
        public void ShouldComputeZIndexOfGrandChildren()
        {
            var skiaParent = new SkiaElement();

            var skiaChild1 = new SkiaElement();
            var skiaChild2 = new SkiaElement();
            var skiaChild3 = new SkiaElement();

            skiaParent.AddChild(skiaChild1);
            skiaParent.AddChild(skiaChild2);
            skiaParent.AddChild(skiaChild3);

            Assert.Greater(skiaChild3.ZIndex, skiaChild2.ZIndex);
            Assert.Greater(skiaChild2.ZIndex, skiaChild1.ZIndex);

            Assert.Greater(skiaChild1.ZIndex, skiaParent.ZIndex);
            Assert.Greater(skiaChild2.ZIndex, skiaParent.ZIndex);
            Assert.Greater(skiaChild3.ZIndex, skiaParent.ZIndex);

            var skiaChild11 = new SkiaElement();
            var skiaChild21 = new SkiaElement();
            var skiaChild31 = new SkiaElement();

            skiaChild1.AddChild(skiaChild11);
            skiaChild2.AddChild(skiaChild21);
            skiaChild3.AddChild(skiaChild31);

            Assert.Greater(skiaChild11.ZIndex, skiaChild1.ZIndex);
            Assert.Greater(skiaChild21.ZIndex, skiaChild2.ZIndex);
            Assert.Greater(skiaChild31.ZIndex, skiaChild3.ZIndex);

            var skiaChild211 = new SkiaElement();

            skiaChild21.AddChild(skiaChild211);

            Assert.Greater(skiaChild211.ZIndex, skiaChild21.ZIndex);
            Assert.Greater(skiaChild3.ZIndex, skiaChild21.ZIndex);
            Assert.Greater(skiaChild3.ZIndex, skiaChild211.ZIndex);


        }


        [Test]
        public void ShouldComputeZIndexOfLotsOfGrandChildren()
        {
            var skiaParent = new SkiaElement();

            var skiaChild1 = new SkiaElement();
            var skiaChild2 = new SkiaElement();
            var skiaChild3 = new SkiaElement();

            skiaParent.AddChild(skiaChild1);
            skiaParent.AddChild(skiaChild2);
            skiaParent.AddChild(skiaChild3);

        
            var skiaChild11 = new SkiaElement();
            var skiaChild12 = new SkiaElement();
            var skiaChild13 = new SkiaElement();
            var skiaChild14 = new SkiaElement();
            var skiaChild15= new SkiaElement();
            var skiaChild16 = new SkiaElement();
            var skiaChild17 = new SkiaElement();
            var skiaChild18 = new SkiaElement();
            var skiaChild19 = new SkiaElement();
            var skiaChild110 = new SkiaElement();
            var skiaChild111 = new SkiaElement();

            skiaChild1.AddChild(skiaChild11);
            skiaChild1.AddChild(skiaChild12);
            skiaChild1.AddChild(skiaChild13);
            skiaChild1.AddChild(skiaChild14);
            skiaChild1.AddChild(skiaChild15);
            skiaChild1.AddChild(skiaChild16);
            skiaChild1.AddChild(skiaChild17);
            skiaChild1.AddChild(skiaChild18);
            skiaChild1.AddChild(skiaChild19);
            skiaChild1.AddChild(skiaChild110);
            skiaChild1.AddChild(skiaChild111);

            Assert.Greater(skiaChild2.ZIndex, skiaChild111.ZIndex);


        }

    }
}
