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

        private interface IPet<out T>
        {
            T RunAway();
        }

        private class Pet<T> : IPet<T>
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


        Pet<Dog> _dog;

        [TestInitialize]
        public void InitializeTest()
        {
            _dog = Pet<Dog>.Domesticate( new Dog() );
        }

        #endregion  // Common Test Members


        [TestMethod]
        public void WithoutReflectionDelegateTest()
        {
            IPet<Dog> test = Pet<Dog>.Domesticate( new Dog() );
            IPet<AbstractAnimal> animal = test;

            // Covarience for generic parameters in .NET 4.0
            Func<Dog> looseDog = _dog.RunAway;
            Func<AbstractAnimal> anyAnimal = looseDog; // After all, a dog which isn't a pet anymore is not just a dog, but also an animal.
            Func<AbstractAnimal> oldFunc = () => looseDog();   // Prior to .NET 4.0
            anyAnimal();
            oldFunc();

            // Contravariance for generic parameters in .NET 4.0
            Action<Dog> play = _dog.PlayWith;
            Action<Bulldog> bullDogPlay = play; // After all, if dogs can get along, bulldogs can get along as well.
            Action<Bulldog> oldAction = friend => play( friend );    // Prior to .NET 4.0
            bullDogPlay( new Bulldog() );
            oldAction( new Bulldog() );

            // Upcasting, so the specific type is known. Force contravariance for one type.
            Func<Dog> assumeDog = () => (Dog)anyAnimal();
            assumeDog();
            assumeDog = () => (Dog)(AbstractAnimal)new Cat();
            AssertHelper.ThrowsException<InvalidCastException>( () => assumeDog() );

            // Upcasting, so the specific type doesn't need to be known. Force covariance for one type.
            Action<AbstractAnimal> assumeDogAction = d => play( (Dog)d );
            assumeDogAction( new Dog() );
            AssertHelper.ThrowsException<InvalidCastException>( () => assumeDogAction( new Cat() ) );
        }

        [TestMethod]
        public void OrdinaryCreateDelegateTest()
        {
            // Action, no template arguments, so no problem.
            MethodInfo feedMethod = _dog.GetType().GetMethod( FeedMethodName );
            Action feed = (Action)Delegate.CreateDelegate( typeof( Action ), _dog, feedMethod );
            feed();

            // Func<T> with known type, and covariance.
            MethodInfo runAwayMethod = _dog.GetType().GetMethod( RunAwayMethodName );
            Func<Dog> runAway = (Func<Dog>)Delegate.CreateDelegate( typeof( Func<Dog> ), _dog, runAwayMethod );
            Func<AbstractAnimal> covariant
                = (Func<AbstractAnimal>)Delegate.CreateDelegate( typeof( Func<AbstractAnimal> ), _dog, runAwayMethod );
            runAway();
            covariant();

            // Action<T> with known type, and contraviance.
            MethodInfo playMethod = _dog.GetType().GetMethod( PlayWithMethodName );
            Action<Dog> playWithDog = (Action<Dog>)Delegate.CreateDelegate( typeof( Action<Dog> ), _dog, playMethod );
            Action<Bulldog> playWithBulldog = (Action<Bulldog>)Delegate.CreateDelegate( typeof( Action<Bulldog> ), _dog, playMethod );
            playWithDog( new Dog() );
            playWithBulldog( new Bulldog() );

            // Upcasting, so the specific type doesn't need to be known. Force covariance for one type.
            AssertHelper.ThrowsException<ArgumentException>(
                () => Delegate.CreateDelegate( typeof( Action<AbstractAnimal> ), _dog, playMethod ) );
        }

        [TestMethod]
        public void CreateUpcastingDelegateTest()
        {
            // Downcasting, so the specific type doesn't need to be known. Force covariance for one type.
            MethodInfo playMethod = _dog.GetType().GetMethod( PlayWithMethodName );
            Action<AbstractAnimal> play = DelegateHelper.CreateDelegate<Action<AbstractAnimal>>(
                playMethod, _dog, DelegateHelper.CreateOptions.Downcasting );

            // No need to know about the exact type during reflection! As long as you are sure it is the right object.
            play( new Dog() );
            AssertHelper.ThrowsException<InvalidCastException>( () => play( new Cat() ) );
        }

        [TestMethod]
        public void OrdinaryDynamicInstanceCreateDelegateTest()
        {
            // Action
            Action<Pet<Dog>> feed = (Action<Pet<Dog>>)Delegate.CreateDelegate(
                typeof( Action<Pet<Dog>> ),
                _dog.GetType().GetMethod( FeedMethodName ) );
            feed( _dog );

            // Action<T>
            Action<Pet<Dog>, Dog> playWith = (Action<Pet<Dog>, Dog>)Delegate.CreateDelegate(
                typeof( Action<Pet<Dog>, Dog> ),
                _dog.GetType().GetMethod( PlayWithMethodName ) );
            playWith( _dog, new Dog() );

            // Func<T>
            Func<Pet<Dog>, Dog> runAway = (Func<Pet<Dog>, Dog>)Delegate.CreateDelegate(
                typeof( Func<Pet<Dog>, Dog> ),
                _dog.GetType().GetMethod( RunAwayMethodName ) );
            runAway( _dog );
        }

        [TestMethod]
        public void CreateUpcastingDynamicInstanceDelegateTest()
        {
            // Downcasting, so the specific type doesn't need to be known. Force covariance for one type.
            MethodInfo playMethod = _dog.GetType().GetMethod( PlayWithMethodName );
            Action<object, AbstractAnimal> play = DelegateHelper.CreateOpenInstanceDelegate<Action<object, AbstractAnimal>>(
                playMethod, DelegateHelper.CreateOptions.Downcasting );

            // No need to know about the exact type during reflection! As long as you are sure it is the right object.
            play( _dog, new Dog() );
            AssertHelper.ThrowsException<InvalidCastException>( () => play( _dog, new Cat() ) );
        }
    }
}
