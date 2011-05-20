using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System;


namespace Whathecode.Tests.System
{
    /// <summary>
    ///   Unit tests for <see href="DelegateHelper">DelegateHelper</see>.
    /// </summary>
    /// <author>Steven Jeuris</author>
    [TestClass]
    public class DelegateHelperTest
    {
        #region Common Test Members

        const string FeedMethodName = "Feed";
        const string PlayWithMethodName = "PlayWith";
        const string RunAwayMethodName = "RunAway";

        private abstract class AbstractAnimal
        {
            public bool Hungry { get; set; }
            public bool Happy { get; set; }

            protected AbstractAnimal()
            {
                Hungry = true;
                Happy = false;
            }
        }

        private class Dog : AbstractAnimal { }
        private class Bulldog : Dog { }
        private class Cat : AbstractAnimal { }

        private class Pet<T>
            where T : AbstractAnimal, new()
        {
            readonly T _animal;

            private Pet( T animal )
            {
                _animal = animal;
            }

            public static Pet<T> Domesticate( T animal )
            {
                return new Pet<T>( animal );
            }

            public void Feed()
            {                
            }

            public void PlayWith( T friend )
            {
                _animal.Happy = true;
            }

            public T RunAway()
            {
                // No longer a pet!
                return _animal;
            }
        }        


        Pet<Dog> _plainDog;
        Pet<Bulldog> _bulldog;
        Pet<Cat> _cat;

        [TestInitialize]
        public void InitializeTest()
        {
            _plainDog = Pet<Dog>.Domesticate( new Dog() );
            _bulldog = Pet<Bulldog>.Domesticate( new Bulldog() );
            _cat = Pet<Cat>.Domesticate( new Cat() );
        }

        #endregion  // Common Test Members


        [TestMethod]
        public void WithoutReflectionDelegateTest()
        {
            // Covarience for generic parameters in .NET 4.0
            Func<Bulldog> looseBulldog = _bulldog.RunAway;
            Func<Dog> anyDog = looseBulldog; // After all, a bulldog which isn't a pet is just a dog as well.
            Func<Dog> oldFunc = () => looseBulldog();   // Prior to .NET 4.0
            anyDog();
            oldFunc();

            // Contravariance for generic parameters in .NET 4.0
            Action<Dog> play = _plainDog.PlayWith;
            Action<Bulldog> bullDogPlay = play; // After all, if dogs can get along, bulldogs can get along as well.
            Action<Bulldog> oldAction = friend => play( friend );    // Prior to .NET 4.0
            bullDogPlay( new Bulldog() );
            oldAction( new Bulldog() );

            // Upcasting, so the specific type is known. Force contravariance for one type.
            Func<Bulldog> assumeBulldog = () => (Bulldog)anyDog();
            assumeBulldog();
            assumeBulldog = () => (Bulldog)new Dog();
            AssertHelper.ThrowsException<InvalidCastException>( () => assumeBulldog() );

            // Upcasting, so the specific type doesn't need to be known. Force covariance for one type.
            Action<Dog> assumeBulldogAction = b => bullDogPlay( (Bulldog)b );
            assumeBulldogAction( new Bulldog() );
            AssertHelper.ThrowsException<InvalidCastException>( () => assumeBulldogAction( new Dog() ) );
        }

        [TestMethod]
        public void OrdinaryCreateDelegateTest()
        {
            // Action, no template arguments, so no problem.
            MethodInfo feedMethod = _plainDog.GetType().GetMethod( FeedMethodName );
            Action feed = (Action)Delegate.CreateDelegate( typeof( Action ), _plainDog, feedMethod );
            feed();

            // Func<T> with known type, and covariance.
            MethodInfo runAwayMethod = _bulldog.GetType().GetMethod( RunAwayMethodName );
            Func<Bulldog> runAway = (Func<Bulldog>)Delegate.CreateDelegate( typeof( Func<Bulldog> ), _bulldog, runAwayMethod );
            Func<Dog> covariant = (Func<Dog>)Delegate.CreateDelegate( typeof( Func<Dog> ), _bulldog, runAwayMethod );
            runAway();
            covariant();

            // Action<T> with known type, and contraviance.
            MethodInfo playMethod = _plainDog.GetType().GetMethod( PlayWithMethodName );
            Action<Dog> playWithDog = (Action<Dog>)Delegate.CreateDelegate( typeof( Action<Dog> ), _plainDog, playMethod );
            Action<Bulldog> playWithBulldog = (Action<Bulldog>)Delegate.CreateDelegate( typeof( Action<Bulldog> ), _plainDog, playMethod );
            playWithDog( new Dog() );
            playWithBulldog( new Bulldog() );

            // Upcasting, so the specific type doesn't need to be known. Force covariance for one type.
            MethodInfo bulldogPlayMethod = _bulldog.GetType().GetMethod( PlayWithMethodName );
            AssertHelper.ThrowsException<ArgumentException>( () => Delegate.CreateDelegate( typeof( Action<Dog> ), _bulldog, playMethod ) );
        }

        [TestMethod]
        public void CreateUpcastingDelegateTest()
        {
            // Upcasting, so the specific type doesn't need to be known. Force covariance for one type.
            MethodInfo playMethod = _bulldog.GetType().GetMethod( PlayWithMethodName );
            Action<Dog> play = DelegateHelper.CreateUpcastingDelegate<Action<Dog>>( _bulldog, playMethod );

            // No need to know about the exact type during reflection! As long as you are sure it is the right object.
            play( new Bulldog() );
            AssertHelper.ThrowsException<InvalidCastException>( () => play( new Dog() ) );
        }
    }
}
