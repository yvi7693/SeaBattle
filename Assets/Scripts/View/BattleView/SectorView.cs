using UnityEngine;

public class SectorView : MonoBehaviour
{
    private int x;
    private int y;
    private BattlePresenter battlePresenter;
    private ParticleSystem fire;
    private ParticleSystem explosion;
    private ParticleSystem splash;
    private SpriteRenderer spriteRenderer;
    private BoxCollider boxCollider;
    private bool setActiveResolved;

    public void Init(int x, int y, BattlePresenter battlePresenter, ParticleSystem fire = null)
    {
        this.x = x;
        this.y = y;
        this.battlePresenter = battlePresenter;
        this.fire = fire;
        setActiveResolved = true;

        splash = this.transform.Find("Splash").GetComponentInChildren<ParticleSystem>();
        explosion = this.transform.Find("Explosion").GetComponentInChildren<ParticleSystem>();
        
    }

    public (int, int) GetCoord()
    {
        return (x,y);
    }

    public bool IsSetActiveResolved()
    {
        return setActiveResolved;
    }


    public void BlowUp()
    {
        ExplosionOn();
        FireOn();
    }


    public void FireOn()
    {
        if (fire != null)
            fire.Play();
    }


    public void ExplosionOn()
    {
        if(explosion != null)
            explosion.Play();
    }


    public void SplashOn()
    {
        if(splash != null)
            splash.Play();
    }


    public void SetExplosion(ParticleSystem explosion)
    {
        this.explosion = explosion;
    }


    public void SetFire(ParticleSystem fire)
    {
        this.fire = fire;
    }


    public void SetSplash(ParticleSystem splash)
    {
        this.splash = splash;
    }


    public void DisplayMiss()
    {
        setActiveResolved = false;
        SetClicked(false);
        spriteRenderer.color = Color.white;
    }

    public void DisplayHit()
    {   
        setActiveResolved = false;
        SetClicked(false);
        spriteRenderer.color = Color.red;
        FireOn();
    }
        

    public void ShowShip()
    {
        spriteRenderer.color = Color.gray;
    }

    public void SetClicked(bool value)
    {
        boxCollider.enabled = value;
    }
   
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnMouseDown()
    {   
        battlePresenter.AttackSector(x, y);
    }

    
}