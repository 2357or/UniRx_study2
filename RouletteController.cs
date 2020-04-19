using System.Collections;
using UnityEngine;
using UniRx;
using System;

public class RouletteController :MonoBehaviour
{
    [SerializeField] private float maxSpeed = 1000, acceleration = 1.1f, deceleration = 0.99f;

    void Start() {
        Observable.FromCoroutine<float>(observer => RouletteSystem(observer, maxSpeed, acceleration, deceleration))
                  .Subscribe(speed => transform.Rotate(0, 0, speed * Time.deltaTime));
    }

    IEnumerator RouletteSystem(IObserver<float> observer, float _maxSpeed, float _acceleration, float _deceleration) {
        float currentSpeed = 1;

        //ルーレットの加速
        while (currentSpeed < _maxSpeed) {
            currentSpeed *= _acceleration;

            observer.OnNext(currentSpeed);

            yield return null;
            Debug.Log(currentSpeed);
        }
        currentSpeed = _maxSpeed;
        observer.OnNext(currentSpeed);

        //ストップの入力待ち
        bool Continue = true;
        while (Continue) {
            if (Input.GetMouseButtonDown(0)) {
                Continue = false;
                observer.OnNext(_maxSpeed);
            }

            observer.OnNext(_maxSpeed);

            Debug.Log(currentSpeed);

            yield return null;
        }

        //減速処理
        while (currentSpeed > 1) {
            currentSpeed *= _deceleration;

            observer.OnNext(currentSpeed);

            yield return null;
            Debug.Log(currentSpeed);

        }
        observer.OnNext(0);


        observer.OnCompleted();
    }
}
