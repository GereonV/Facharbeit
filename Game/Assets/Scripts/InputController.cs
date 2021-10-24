using UnityEngine;

namespace InputManagement {

    public interface InputController {

        Vector2 GetInput();

    }

    public class PlayerInputController : InputController {

        Vector2 InputController.GetInput() {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

    }

    public class NetworkInputController : InputController {

        Vector2 InputController.GetInput() {
            return Vector2.zero;
        }

    }

}
