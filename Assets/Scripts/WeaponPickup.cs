using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Tooltip("Ajusta esta posición si el arma no queda bien alineada")]
    public Vector3 positionOffset = Vector3.zero;

    [Tooltip("Ajusta esta rotación si el arma no queda bien orientada")]
    public Vector3 rotationOffset = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Buscar el punto de sujeción en el jugador
            Transform holder = other.transform.Find("WeaponHolder");

            if (holder != null)
            {
                AttachToHolder(holder);
            }
            else
            {
                // Si no encuentra WeaponHolder, usa un punto alternativo
                Debug.LogWarning("No se encontró WeaponHolder, usando transform del player");
                AttachToHolder(other.transform);
            }
        }
    }

    private void AttachToHolder(Transform holder)
    {
        // Desactivar física si existe
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Configurar parent y transformación relativa
        transform.SetParent(holder);
        if (holder.GetComponent<WeaponController>() != null)
        {
            WeaponController weaponController = holder.GetComponent<WeaponController>();
            weaponController.hasWeapon = true;
        }
        else
        {
            WeaponController1 weaponController = holder.GetComponent<WeaponController1>();
            weaponController.hasWeapon = true;
        }

        transform.localPosition = positionOffset;
        transform.localEulerAngles = rotationOffset;
        
        // Desactivar collider para evitar nuevas detecciones
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        
        // Opcional: Desactivar este script si ya no es necesario
        // enabled = false;
    }
}
