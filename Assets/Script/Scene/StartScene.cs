using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("UI Scene"); // ���� ������ �̵�
    }

    public void OpenOption()
    {
        Debug.Log("�ɼ�â ���� (���߿� ����)");
    }

    public void ExitGame()
    {
        Debug.Log("���� ����");

        // ������ ȯ�濡���� ���ᰡ �� �ǹǷ� ���� �б�
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �÷��� ��� ����
#else
        Application.Quit(); // ����� ���� ���Ͽ����� ������ �����
#endif
    }
}
