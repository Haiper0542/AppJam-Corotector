# Corotector
* #### 장르 : VR 슈팅 게임
* #### 역할 : 프로그래머
* #### 개발 언어, 툴 : C#, Unity
* #### 사용 기기 : HTC Vive

## 프로젝트 소개
### 게임 소개
* 제 17회 앱잼에서 제작되었습니다.
* 해당 게임은 VR 슈팅 게임입니다.
* 플레이어는 가디언과 서프라이즈 2가지 클래스 중 하나를 택할 수 있습니다.
* 건물 중앙에 있는 코어를 지키는 것이 게임의 목표이며, 드론이 접근하기전에 모두 추락 시켜야합니다.
* 코어의 체력이 모두 소모될 경우 게임 오버됩니다.
+ **클래스**
   + **가디언**
      + 방패와 권총을 하나 들고 있는 클래스 입니다.
      + 공격력은 낮지만 방패로 드론이 발사하는 총알을 방어할 수 있습니다.
   + **서프라이즈**
      + 쌍권총을 들고 있는 클래스입니다.
      + 공격력이 매우 강합니다.
* **드론**
  * **일반 드론**
      * 플레이어 혹은 코어중 하나를 타겟으로 정합니다.
      * 접근시 3연발 총을 사용하여 공격하며, 체력은 중간 정도입니다.
  * **자폭 드론**
      * 코어만을 타겟으로 정합니다.
      * 접근시 폭발하여 강력한 데미지를 입히며, 체력은 중간 정도입니다.
  * **스나이퍼 드론**
      * 플레이어 혹은 코어중 하나를 타겟으로 정합니다.
      * 멀리서 강력한 데미지를 주는 총을 한 발씩 발사하며, 체력은 낮니다.
      
### 시연 영상
* [https://youtu.be/wPs0yXkAS_4](https://youtu.be/wPs0yXkAS_4) <br>

### 게임 화면
* **게임의 메인화면입니다.**<br>
<img width="70%" src=https://user-images.githubusercontent.com/40797534/56399107-19889800-6287-11e9-9c29-b70f75fe194c.png></img>

* **가디언 혹은 서프라이즈 클래스를 선택할 수 있습니다.**
<img width="70%" src=https://user-images.githubusercontent.com/40797534/56399115-1e4d4c00-6287-11e9-82f7-877a88092cf6.png></img>
<img width="70%" src=https://user-images.githubusercontent.com/40797534/56399108-1a212e80-6287-11e9-92a4-4e48837cc5a0.png></img>

* **가디언은 왼쪽에 있는 방패로 적캐릭터의 공격을 막을 수 있습니다.**
<img width="70%" src=https://user-images.githubusercontent.com/40797534/56399110-1ab9c500-6287-11e9-9684-1505fbf38489.png></img>
<img width="70%" src=https://user-images.githubusercontent.com/40797534/56399111-1beaf200-6287-11e9-96c3-c4af4f3031fc.png></img>
<img width="70%" src=https://user-images.githubusercontent.com/40797534/56399112-1c838880-6287-11e9-80c3-ca0d11a0b688.png></img>

* **오른쪽에 있는 권총으로 적캐릭터를 추락시킬 수 있습니다.**
<img width="70%" src=https://user-images.githubusercontent.com/40797534/56399113-1d1c1f00-6287-11e9-8292-3d5a03b30cef.png></img>
<img width="70%" src=https://user-images.githubusercontent.com/40797534/56399114-1db4b580-6287-11e9-9294-af32e4b70135.png></img>

* **서프라이즈는 양손에 총을 들고 있으며 매우 발사 속도가 빠릅니다.**
<img width="70%" src=https://user-images.githubusercontent.com/40797534/56399116-1ee5e280-6287-11e9-82e6-5bf7d4637650.png></img>

* **게임오버시 화면입니다.**<br>
<img width="70%" src=https://user-images.githubusercontent.com/40797534/56399094-0fff3000-6287-11e9-84e8-7155a69f006e.png></img>

## 코드
* #### 일반 드론 공격 부분
```C#
if (attackTime > attackDelay)
   {
   attackTime = 0;
   bulletCount--;
   try
   {
      audioSource.PlayOneShot(shotClip); //효과음 추가 할꺼야???
   }
   catch {  }
            
   if (Physics.Raycast(muzzle.position, muzzle.rotation * Vector3.forward, out hit, enemyLayer))
   {
      Debug.Log(hit.transform.name);
      if (hit.transform.CompareTag("Player"))
         hit.transform.GetComponent<PlayerCtrl>().TakeDamage(damage);
      if (hit.transform.CompareTag("Core"))
         hit.transform.GetComponent<CoreCtrl>().TakeDamage(damage);
      //방패로 막을 시 스파크
      if (hit.transform.CompareTag("Shield"))
      {
         audioSource.PlayOneShot(shieldClip);
         Instantiate(spark, hit.point, Quaternion.Euler(hit.normal));
      }
   }

   //
   if (bulletCount <= 0)
   {
      reloadTime = reloadTerm;
      isMoving = true;
      isReloading = true;
      demoLaser.SetActive(true);
   }
}
```
