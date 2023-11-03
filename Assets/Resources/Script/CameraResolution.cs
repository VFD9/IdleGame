using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    int width;
    int height;

    private void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        width = Screen.width;
        height = Screen.height;
        Screen.SetResolution(width, height, true);
    }
}

// ((float)Screen.width / Screen.height) => ���� �ڵ����� ���� / ����, ((float)9 / 16) => �����Ϸ��� ȭ�� ����
// (���� / ����), ������ �ƴҰ�� �Ҽ����� ���ֱ⶧���� float�� �ٿ���
// ���η� ������ �� ��� ((float)9 / 16)�� �ƴ� ((float)16 / 9)�� �ؾ��Ѵ�.
// ������ ���κ��� scaleheight�̺��� ũ�� ������ �� 1���� ū ���� ������ scaleheight�̺��� ������ 1���� ���� ���� ���´�.
//float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9); 
//float scalewidth = 1f / scaleheight;
//if (scaleheight < 1)    // �����Ϸ��� ȭ�� �������� ����Ƽ���� ������ ������ ������ ���Ʒ��� ���´�.
//{
//    rect.height = scaleheight;          // ���Ʒ����� ������ ȭ���� �����.
//    rect.y = (1f - scaleheight) / 2f;   // y ���� ������ ���� ������ ���ش�.
//}
//else                    // �����Ϸ��� ȭ�� �������� ����Ƽ���� ������ ������ ũ�� �¿찡 ���´�.
//{
//    rect.width = scalewidth;            // �¿츦 ������ ȭ���� �����.
//    rect.x = (1f - scalewidth) / 2f;    // x ���� ������ ���� ������ ���ش�.
//}
//camera.rect = rect;     // �ٲ� rect�� ī�޶� rect�� �ٽ� �����Ѵ�.