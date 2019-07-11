using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuralUIController : MonoBehaviour
{
    [SerializeField]
    Sprite nodeSprite;

    [SerializeField]
    RectTransform graphicContainer;

    [SerializeField]
    AnimalPicker animalPicker;

    [SerializeField]
    Text[] graphicalInputValues;

    [SerializeField]
    Text[] graphicalOutputValues;

    [SerializeField]
    bool showValues = true;

    float minWeight = -5f;
    float maxWeight = 5f;

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    GameObject AddNode(Vector2 anchoredPosition, Color color)
    {
        GameObject gameObject = new GameObject("node", typeof(Image));
        gameObject.transform.SetParent(graphicContainer, false);
        gameObject.GetComponent<Image>().sprite = nodeSprite;
        gameObject.GetComponent<Image>().color = color;
        gameObject.GetComponent<Image>().useSpriteMesh = true;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(24, 24);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return gameObject;
    }

    GameObject AddNodeConnections(GameObject nodeA, GameObject nodeB, Color color)
    {
        GameObject gameObject = new GameObject("connection", typeof(Image));
        gameObject.transform.SetParent(graphicContainer, false);
        gameObject.GetComponent<Image>().color = color;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 direction = (nodeB.GetComponent<RectTransform>().anchoredPosition - nodeA.GetComponent<RectTransform>().anchoredPosition).normalized;
        float distance = Vector2.Distance(nodeA.GetComponent<RectTransform>().anchoredPosition, nodeB.GetComponent<RectTransform>().anchoredPosition);

        rectTransform.sizeDelta = new Vector2(distance, 1f);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.anchoredPosition = nodeA.GetComponent<RectTransform>().anchoredPosition + direction * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(direction));

        return gameObject;
    }

    GameObject connection1,  connection2,  connection3,  connection4,  connection5,  connection6,  connection7,  connection8,  connection9,  connection10,  connection11, connection12;
    GameObject connection13, connection14, connection15, connection16, connection17, connection18, connection19, connection20, connection21, connection22, connection23, connection24;
    GameObject connection25, connection26, connection27, connection28, connection29, connection30, connection31, connection32, connection33, connection34, connection35, connection36;
    GameObject connection37, connection38, connection39, connection40, connection41, connection42, connection43, connection44, connection45, connection46, connection47, connection48;
    GameObject connection49, connection50, connection51, connection52, connection53, connection54, connection55, connection56, connection57, connection58, connection59, connection60;
    GameObject connection61, connection62, connection63, connection64, connection65, connection66, connection67, connection68, connection69, connection70, connection71, connection72;
    GameObject connection73, connection74, connection75, connection76, connection77, connection78;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject input1 = AddNode(new Vector2(50, (300 / 10 * 1)), new Color(0, 0, 0));
        GameObject input2 = AddNode(new Vector2(50, (300 / 10 * 2)), new Color(0, 0, 0));
        GameObject input3 = AddNode(new Vector2(50, (300 / 10 * 3)), new Color(0, 0, 0));
        GameObject input4 = AddNode(new Vector2(50, (300 / 10 * 4)), new Color(0, 0, 0));
        GameObject input5 = AddNode(new Vector2(50, (300 / 10 * 5)), new Color(0, 0, 0));
        GameObject input6 = AddNode(new Vector2(50, (300 / 10 * 6)), new Color(0, 0, 0));
        GameObject input7 = AddNode(new Vector2(50, (300 / 10 * 7)), new Color(0, 0, 0));
        GameObject input8 = AddNode(new Vector2(50, (300 / 10 * 8)), new Color(0, 0, 0));
        GameObject input9 = AddNode(new Vector2(50, (300 / 10 * 9)), new Color(0, 0, 0));

        GameObject node1  = AddNode(new Vector2(250, (300 / 7 * 1)), new Color(255, 255, 255));
        GameObject node2  = AddNode(new Vector2(250, (300 / 7 * 2)), new Color(255, 255, 255));
        GameObject node3  = AddNode(new Vector2(250, (300 / 7 * 3)), new Color(255, 255, 255));
        GameObject node4  = AddNode(new Vector2(250, (300 / 7 * 4)), new Color(255, 255, 255));
        GameObject node5  = AddNode(new Vector2(250, (300 / 7 * 5)), new Color(255, 255, 255));
        GameObject node6  = AddNode(new Vector2(250, (300 / 7 * 6)), new Color(255, 255, 255));

        GameObject output1 = AddNode(new Vector2(450,  60), new Color(0, 255, 0));
        GameObject output2 = AddNode(new Vector2(450, 120), new Color(0, 255, 0));
        GameObject output3 = AddNode(new Vector2(450, 180), new Color(0, 255, 0));
        GameObject output4 = AddNode(new Vector2(450, 240), new Color(0, 255, 0));

        //-- INPUT CONNECTIONS --//
        connection1  = AddNodeConnections(input1, node1, new Color(0, 0, 0));
        connection2  = AddNodeConnections(input1, node2, new Color(0, 0, 0));
        connection3  = AddNodeConnections(input1, node3, new Color(0, 0, 0));
        connection4  = AddNodeConnections(input1, node4, new Color(0, 0, 0));
        connection5  = AddNodeConnections(input1, node5, new Color(0, 0, 0));
        connection6  = AddNodeConnections(input1, node6, new Color(0, 0, 0));

        connection7  = AddNodeConnections(input2, node1, new Color(0, 0, 0));
        connection8  = AddNodeConnections(input2, node2, new Color(0, 0, 0));
        connection9  = AddNodeConnections(input2, node3, new Color(0, 0, 0));
        connection10 = AddNodeConnections(input2, node4, new Color(0, 0, 0));
        connection11 = AddNodeConnections(input2, node5, new Color(0, 0, 0));
        connection12 = AddNodeConnections(input2, node6, new Color(0, 0, 0));

        connection13 = AddNodeConnections(input3, node1, new Color(0, 0, 0));
        connection14 = AddNodeConnections(input3, node2, new Color(0, 0, 0));
        connection15 = AddNodeConnections(input3, node3, new Color(0, 0, 0));
        connection16 = AddNodeConnections(input3, node4, new Color(0, 0, 0));
        connection17 = AddNodeConnections(input3, node5, new Color(0, 0, 0));
        connection18 = AddNodeConnections(input3, node6, new Color(0, 0, 0));

        connection19 = AddNodeConnections(input4, node1, new Color(0, 0, 0));
        connection20 = AddNodeConnections(input4, node2, new Color(0, 0, 0));
        connection21 = AddNodeConnections(input4, node3, new Color(0, 0, 0));
        connection22 = AddNodeConnections(input4, node4, new Color(0, 0, 0));
        connection23 = AddNodeConnections(input4, node5, new Color(0, 0, 0));
        connection24 = AddNodeConnections(input4, node6, new Color(0, 0, 0));

        connection25 = AddNodeConnections(input5, node1, new Color(0, 0, 0));
        connection26 = AddNodeConnections(input5, node2, new Color(0, 0, 0));
        connection27 = AddNodeConnections(input5, node3, new Color(0, 0, 0));
        connection28 = AddNodeConnections(input5, node4, new Color(0, 0, 0));
        connection29 = AddNodeConnections(input5, node5, new Color(0, 0, 0));
        connection30 = AddNodeConnections(input5, node6, new Color(0, 0, 0));

        connection31 = AddNodeConnections(input6, node1, new Color(0, 0, 0));
        connection32 = AddNodeConnections(input6, node2, new Color(0, 0, 0));
        connection33 = AddNodeConnections(input6, node3, new Color(0, 0, 0));
        connection34 = AddNodeConnections(input6, node4, new Color(0, 0, 0));
        connection35 = AddNodeConnections(input6, node5, new Color(0, 0, 0));
        connection36 = AddNodeConnections(input6, node6, new Color(0, 0, 0));

        connection37 = AddNodeConnections(input7, node1, new Color(0, 0, 0));
        connection38 = AddNodeConnections(input7, node2, new Color(0, 0, 0));
        connection39 = AddNodeConnections(input7, node3, new Color(0, 0, 0));
        connection40 = AddNodeConnections(input7, node4, new Color(0, 0, 0));
        connection41 = AddNodeConnections(input7, node5, new Color(0, 0, 0));
        connection42 = AddNodeConnections(input7, node6, new Color(0, 0, 0));

        connection43 = AddNodeConnections(input8, node1, new Color(0, 0, 0));
        connection44 = AddNodeConnections(input8, node2, new Color(0, 0, 0));
        connection45 = AddNodeConnections(input8, node3, new Color(0, 0, 0));
        connection46 = AddNodeConnections(input8, node4, new Color(0, 0, 0));
        connection47 = AddNodeConnections(input8, node5, new Color(0, 0, 0));
        connection48 = AddNodeConnections(input8, node6, new Color(0, 0, 0));

        connection49 = AddNodeConnections(input9, node1, new Color(0, 0, 0));
        connection50 = AddNodeConnections(input9, node2, new Color(0, 0, 0));
        connection51 = AddNodeConnections(input9, node3, new Color(0, 0, 0));
        connection52 = AddNodeConnections(input9, node4, new Color(0, 0, 0));
        connection53 = AddNodeConnections(input9, node5, new Color(0, 0, 0));
        connection54 = AddNodeConnections(input9, node6, new Color(0, 0, 0));

        //-- OUTPUT CONNECTIONS --//
        connection55 = AddNodeConnections(node1, output1, new Color(0, 0, 0));
        connection56 = AddNodeConnections(node1, output2, new Color(0, 0, 0));
        connection57 = AddNodeConnections(node1, output3, new Color(0, 0, 0));
        connection58 = AddNodeConnections(node1, output4, new Color(0, 0, 0));

        connection59 = AddNodeConnections(node2, output1, new Color(0, 0, 0));
        connection60 = AddNodeConnections(node2, output2, new Color(0, 0, 0));
        connection61 = AddNodeConnections(node2, output3, new Color(0, 0, 0));
        connection62 = AddNodeConnections(node2, output4, new Color(0, 0, 0));

        connection63 = AddNodeConnections(node3, output1, new Color(0, 0, 0));
        connection64 = AddNodeConnections(node3, output2, new Color(0, 0, 0));
        connection65 = AddNodeConnections(node3, output3, new Color(0, 0, 0));
        connection66 = AddNodeConnections(node3, output4, new Color(0, 0, 0));

        connection67 = AddNodeConnections(node4, output1, new Color(0, 0, 0));
        connection68 = AddNodeConnections(node4, output2, new Color(0, 0, 0));
        connection69 = AddNodeConnections(node4, output3, new Color(0, 0, 0));
        connection70 = AddNodeConnections(node4, output4, new Color(0, 0, 0));

        connection71 = AddNodeConnections(node5, output1, new Color(0, 0, 0));
        connection72 = AddNodeConnections(node5, output2, new Color(0, 0, 0));
        connection73 = AddNodeConnections(node5, output3, new Color(0, 0, 0));
        connection74 = AddNodeConnections(node5, output4, new Color(0, 0, 0));

        connection75 = AddNodeConnections(node6, output1, new Color(0, 0, 0));
        connection76 = AddNodeConnections(node6, output2, new Color(0, 0, 0));
        connection77 = AddNodeConnections(node6, output3, new Color(0, 0, 0));
        connection78 = AddNodeConnections(node6, output4, new Color(0, 0, 0));
    }
    
    void SetColor(GameObject gameObject, Color color)
    {
        gameObject.GetComponent<Image>().color = color;
    }

    private void Update()
    {

        //INPUT UPDATE
        SetColor(connection1, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[0, 0])));
        SetColor(connection2, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[0, 1])));
        SetColor(connection3, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[0, 2])));
        SetColor(connection4, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[0, 3])));
        SetColor(connection5, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[0, 4])));
        SetColor(connection6, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[0, 5])));

        SetColor(connection7,  Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[1, 0])));
        SetColor(connection8,  Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[1, 1])));
        SetColor(connection9,  Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[1, 2])));
        SetColor(connection10, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[1, 3])));
        SetColor(connection11, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[1, 4])));
        SetColor(connection12, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[1, 5])));

        SetColor(connection13, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[2, 0])));
        SetColor(connection14, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[2, 1])));
        SetColor(connection15, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[2, 2])));
        SetColor(connection16, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[2, 3])));
        SetColor(connection17, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[2, 4])));
        SetColor(connection18, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[2, 5])));

        SetColor(connection19, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[3, 0])));
        SetColor(connection20, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[3, 1])));
        SetColor(connection21, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[3, 2])));
        SetColor(connection22, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[3, 3])));
        SetColor(connection23, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[3, 4])));
        SetColor(connection24, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[3, 5])));

        SetColor(connection25, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[4, 0])));
        SetColor(connection26, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[4, 1])));
        SetColor(connection27, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[4, 2])));
        SetColor(connection28, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[4, 3])));
        SetColor(connection29, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[4, 4])));
        SetColor(connection30, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[4, 5])));

        SetColor(connection31, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[5, 0])));
        SetColor(connection32, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[5, 1])));
        SetColor(connection33, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[5, 2])));
        SetColor(connection34, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[5, 3])));
        SetColor(connection35, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[5, 4])));
        SetColor(connection36, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[5, 5])));

        SetColor(connection37, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[6, 0])));
        SetColor(connection38, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[6, 1])));
        SetColor(connection39, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[6, 2])));
        SetColor(connection40, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[6, 3])));
        SetColor(connection41, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[6, 4])));
        SetColor(connection42, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[6, 5])));

        SetColor(connection43, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[7, 0])));
        SetColor(connection44, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[7, 1])));
        SetColor(connection45, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[7, 2])));
        SetColor(connection46, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[7, 3])));
        SetColor(connection47, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[7, 4])));
        SetColor(connection48, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[7, 5])));

        SetColor(connection49, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[8, 0])));
        SetColor(connection50, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[8, 1])));
        SetColor(connection51, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[8, 2])));
        SetColor(connection52, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[8, 3])));
        SetColor(connection53, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[8, 4])));
        SetColor(connection54, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.inputLayer.weights[8, 5])));

        //OUTPUT UPDATE
        SetColor(connection55, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[0, 0])));
        SetColor(connection56, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[0, 1])));
        SetColor(connection57, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[0, 2])));
        SetColor(connection58, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[0, 3])));

        SetColor(connection59, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[1, 0])));
        SetColor(connection60, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[1, 1])));
        SetColor(connection61, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[1, 2])));
        SetColor(connection62, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[1, 3])));

        SetColor(connection63, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[2, 0])));
        SetColor(connection64, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[2, 1])));
        SetColor(connection65, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[2, 2])));
        SetColor(connection66, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[2, 3])));

        SetColor(connection67, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[3, 0])));
        SetColor(connection68, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[3, 1])));
        SetColor(connection69, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[3, 2])));
        SetColor(connection70, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[3, 3])));

        SetColor(connection71, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[4, 0])));
        SetColor(connection72, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[4, 1])));
        SetColor(connection73, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[4, 2])));
        SetColor(connection74, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[4, 3])));

        SetColor(connection75, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[5, 0])));
        SetColor(connection76, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[5, 1])));
        SetColor(connection77, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[5, 2])));
        SetColor(connection78, Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(minWeight, maxWeight, NeuralNetwork.Instance.hiddenLayer.weights[5, 3])));

        // Values

        Prey selectedPrey = (Prey) animalPicker.GetAnimal();

        if(showValues && selectedPrey)
        {
            for (int i = 0; i < graphicalInputValues.Length; ++i)
            {
                graphicalInputValues[i].text = selectedPrey.input[i].ToString();
            }
            
            for (int i = 0; i < graphicalOutputValues.Length; ++i)
            {
                graphicalOutputValues[i].text = selectedPrey.output_array[i].ToString();
            }
        }

        else
        {
            for (int i = 0; i < graphicalInputValues.Length; ++i)
            {
                graphicalInputValues[i].text = "";
            }

            for (int i = 0; i < graphicalOutputValues.Length; ++i)
            {
                graphicalOutputValues[i].text = "";
            }
        }


    }
}
