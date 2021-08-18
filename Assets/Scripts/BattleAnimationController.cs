using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct AnimationQueueContainer{

    public AnimationQueueContainer(CharacterInfo _attacker, CharacterInfo _defender)
    {
        attacker = _attacker;
        defender = _defender;
    }

    public CharacterInfo attacker;
    public CharacterInfo defender;
}

public class BattleAnimationController : MonoBehaviour
{

    public bool isAnimating;

    private Queue<AnimationQueueContainer> attackAnimationQueue;

    private void Start()
    {
        isAnimating = false;
        attackAnimationQueue = new Queue<AnimationQueueContainer>();
    }
    #region MOVEMENT_ANIMATIONS
    public void MoveCharacterToPosition(CharacterInfo characterInfo, TileInfo endTile)
    {
        Stack<TileInfo> movementPath = new Stack<TileInfo>();
        int i = endTile.index;

        movementPath.Push(endTile);

        while(characterInfo.shortestPath.parent[i] != -1)
        {
            movementPath.Push(WorldStateInfo.Instance.mapTileGraph.graphNodes.Find(x => x.nodeIndex == characterInfo.shortestPath.parent[i]).tile);
            i = characterInfo.shortestPath.parent[i];
        }

        isAnimating = true;

        StartCoroutine(MoveThroughPath(characterInfo, movementPath));
    }

    private IEnumerator MoveThroughPath(CharacterInfo characterInfo, Stack<TileInfo> path)
    {
        Vector3 nextPosition = path.Pop().transform.position;
        while (path.Count != 0)
        {
            if (characterInfo.transform.position == nextPosition)
            {
                nextPosition = path.Pop().transform.position;
                yield return StartCoroutine(SmoothMovement(nextPosition, characterInfo.GetRigidbody2D(), .10f));
            }
        }

        isAnimating = false;
    }

    private IEnumerator SmoothMovement(Vector3 end, Rigidbody2D rigidBody2D, float speed = .25f)
    {
        float sqrRemainingDistance = (rigidBody2D.transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rigidBody2D.transform.position, end, speed);

            rigidBody2D.MovePosition(newPosition);
            sqrRemainingDistance = (rigidBody2D.transform.position - end).sqrMagnitude;

            yield return null;
        }
        
    }

    #endregion


    #region ATTACK_ANIMATIONS

    public void AttackAnimationProcessing()
    {
        isAnimating = true;
        StartCoroutine(GoThroughAnimationQueue());
    }

    private IEnumerator GoThroughAnimationQueue()
    {
        AnimationQueueContainer currentAnimation;

        while(attackAnimationQueue.Count != 0)
        {
            currentAnimation = attackAnimationQueue.Dequeue();
            yield return StartCoroutine(AttackAnimationFast(currentAnimation.attacker, currentAnimation.defender));
        }
        isAnimating = false;
    }

    private IEnumerator AttackAnimationFast(CharacterInfo attacker, CharacterInfo defender)
    {
        Vector3 attackPosition = attacker.transform.position;
        Vector3 defenderPosition = defender.transform.position;
        Vector3 attackDirection = new Vector3(Mathf.Abs(attackPosition.x - attacker.currentTile.transform.position.x), Mathf.Abs(attackPosition.y - attacker.currentTile.transform.position.y), 0);

        Vector3 toMovePos;

        if((attackDirection.y > attackDirection.x && attackPosition.y - defenderPosition.y != 0) || attackPosition.x - defenderPosition.x == 0)
        {
            float toMoveY = (attackPosition.y - defenderPosition.y) / (Mathf.Abs(attackPosition.y -defenderPosition.y));
            toMovePos = new Vector3(attackPosition.x, attackPosition.y + (toMoveY * .25f), attackPosition.z);
        }
        else if(attackDirection.x >= attackDirection.y || attackPosition.y - defenderPosition.y == 0)
        {
            float toMoveX = (defenderPosition.x - attackPosition.x) / Mathf.Abs(defenderPosition.x - attackPosition.x);
            toMovePos = new Vector3(attackPosition.x + (toMoveX * .5f), attackPosition.y, attackPosition.z);
        }
        else
        {
            float toMoveX = (attackPosition.x - defenderPosition.y) / Mathf.Abs(attackPosition.y - defenderPosition.y);
            toMovePos = new Vector3(attackPosition.x + (toMoveX * .5f), attackPosition.y, attackPosition.z);
        }

        yield return SmoothMovement(toMovePos, attacker.GetRigidbody2D(), .05f);

        yield return new WaitForSeconds(.35f);

        yield return SmoothMovement(attackPosition, attacker.GetRigidbody2D(), .05f);

    }

    public void AddToAnimationQueue(CharacterInfo attacker, CharacterInfo defender)
    {
        attackAnimationQueue.Enqueue(new AnimationQueueContainer(attacker, defender));
    }

    #endregion

    public void isAnimationDone()
    {
        
    }

    public IEnumerator WaitForAnimationEnd()
    {
        yield return new WaitUntil(() => !isAnimating);
    }
}
