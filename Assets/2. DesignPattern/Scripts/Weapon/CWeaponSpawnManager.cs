using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Homework0804
{
    public class CWeaponSpawnManager : MonoBehaviour
    {
        #region public ����
        public List<CWeapon> weaponPrefabs = new List<CWeapon>();
        #endregion

        #region private ����
        [SerializeField]
        Transform tfPlayer;
        #endregion

        public void OnWeaponSpawnButtonClick(int index)
        {
            Vector3 spawnPoint = tfPlayer.position + tfPlayer.forward * 10.0f;

            Instantiate(weaponPrefabs[index], spawnPoint, Quaternion.identity);
        }
    }
}
