using UnityEngine;
using UnityEngine.InputSystem;

public class M1 : MonoBehaviour
{
    // 初始轉速
    float rotSpeed = 0f;

    // 最大轉速
    float maxSpeed = 10f;

    // 減速速度
    float slowDown = 2f;

    // 是否正在減速
    bool isSlowingDown = false;

    // 輪盤停止音效
    public AudioClip stopSound;

    // 音效播放器
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Update()
    {
        // 偵測滑鼠左鍵
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (rotSpeed == 0f && !isSlowingDown)
            {
                // 開始旋轉
                rotSpeed = maxSpeed;
            }
            else if (!isSlowingDown)
            {
                // 開始減速
                isSlowingDown = true;

                // 開始播放減速音效
                if (stopSound != null)
                {
                    audioSource.clip = stopSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
        }

        // 逐漸減速
        if (isSlowingDown)
        {
            rotSpeed = Mathf.MoveTowards(
                rotSpeed,
                0f,
                slowDown * Time.deltaTime
            );

            // 轉盤完全停止
            if (rotSpeed <= 0f)
            {
                rotSpeed = 0f;
                isSlowingDown = false;

                // 停止播放音效
                audioSource.Stop();
                audioSource.loop = false;
                audioSource.clip = null;
            }
        }

        // 旋轉轉盤
        transform.Rotate(
            0f,
            0f,
            -rotSpeed * 60f * Time.deltaTime
        );
    }
}