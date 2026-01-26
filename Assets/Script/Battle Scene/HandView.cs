using UnityEngine;
using UnityEngine.Splines;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;   //spline 저장

    private readonly List<CardView> cards = new();             //cardsView 리스트 생성

    public IEnumerator AddCard(CardView cardView)              //카드 추가 함수 코루틴 실행
    {
        cards.Add(cardView);                                    //cards 리스트에 카드 추가
        yield return UpdateCardPositions(0.15f);                //함수 실행하고 함수가 끝나면 이 함수도 끝
    } 

    public CardView RemoveCard(Card card)
    {
        CardView cardView = GetCardView(card);
        if (cardView == null) return null;
        cards.Remove(cardView);
        StartCoroutine(UpdateCardPositions(0.15f));
        return cardView;
    }

    private CardView GetCardView(Card card)
    {
        return cards.Where(GetCardView => GetCardView.Card == card).FirstOrDefault();
    }

    private IEnumerator UpdateCardPositions(float duration) //duration 동안 이동 애니매이션 진행
    {
        if(cards.Count == 0)yield break;    //카드가 없을때 그냥 코루틴 종료
        float cardSpacing = 0.1f;           //카드간 간격 (스플라인 파라미터의 간격)
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;   //맨 왼쪽 카드 위치
        Spline spline = splineContainer.Spline;     //spline 객체 생성
        for(int i = 0; i < cards.Count; i++)        //손에있는 모든 카드를 0번부터 끝까지 순회
        {
            float p = firstCardPosition + i * cardSpacing; //놓일 위치 파라미터 p 계산
            Vector3 splinePosition = spline.EvaluatePosition(p);    //spline p 지점의 스플라인 기준 로컬좌표
            Vector3 forward = spline.EvaluateTangent(p);            //그 지점의 진행 방향
            Vector3 up = spline.EvaluateUpVector(p);                //그 지점의 위쪽 방향
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);     //카드가 바라볼 회전 방향 계산
            cards[i].transform.DOMove(splinePosition + transform.position + 0.01f * i * Vector3.back, duration);    //DOTween으로 i 번째 카드 duration 동안 이동
            cards[i].transform.DORotate(rotation.eulerAngles, duration);       //DOTween 으로 i 번째 카드 duration동안 회전
        }
        yield return new WaitForSeconds(duration);  //duration 동안 기다렸다가 종료
    }
}
